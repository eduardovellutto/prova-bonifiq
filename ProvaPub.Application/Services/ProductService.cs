using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Contracts;
using ProvaPub.Domain.Exceptions;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly TestDbContext _ctx;

        public ProductService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ProductList> GetListAsync(int? page = null, int pageSize = 10)
        {
            try
            {
                if (page == null)
                    return await GetListWithoutPagination();
                else
                    return await GetListWihtPagination(page, pageSize);
            }
            catch (Exception ex)
            {
                throw new NoAvailableProductsException(ex.Message);
            }
        }

        private async Task<ProductList> GetListWihtPagination(int? page, int pageSize)
        {
            List<Product> products = new List<Product>();

            products = await _ctx.Products
                        .OrderBy(r => r.Id)
                        .Skip((page.Value - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToListAsync();

            int totalItems = await _ctx.Products.CountAsync();
            pageSize = products.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            bool hasNextPage = page < totalPages;

            return new ProductList(products, pageSize, hasNextPage);
        }

        private async Task<ProductList> GetListWithoutPagination()
        {
            List<Product> products = new List<Product>();

            products = await _ctx.Products
                       .OrderBy(r => r.Id)
                       .AsNoTracking()
                       .ToListAsync();

            int pageSize = products.Count;

            return new ProductList(products, pageSize, false);
        }

    }
}
