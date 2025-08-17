using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Contracts;
using ProvaPub.Domain.Exceptions;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Domain.Models.Responses;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TestDbContext _ctx;
        private readonly IPurchaseService _purchaseService;

        public CustomerService(TestDbContext ctx, IPurchaseService purchaseService)
        {
            _ctx = ctx;
            _purchaseService = purchaseService;
        }

        public async Task<CustomerList> GetListAsync(int? page = null, int pageSize = 10)
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

        public async Task<PurchaseResponse> CanPurchase(int customerId, decimal purchaseValue, bool bypassBusinessHour = false)
        {
            //Business Rule: Non registered Customers cannot purchase
            if (!await _purchaseService.CustumerExists(customerId))
                return new PurchaseResponse(false, "Customer does not exist");

            //Business Rule: A customer can purchases only during business hours and working days
            if (!_purchaseService.IsBusinessHours(bypassBusinessHour))
                return new PurchaseResponse(false, "Purchases permitted only during business hours");

            //Business Rule: A customer can purchase only a single time per month
            if (!await _purchaseService.CanPurchaseThisMonth(customerId))
                return new PurchaseResponse(false, "Customer has already made purchases this month");

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            if (!await _purchaseService.IsFirstPurchaseValid(customerId, purchaseValue))
                return new PurchaseResponse(false, "The value of the first purchase must be a maximum of 100.00");

            return new PurchaseResponse(true, "Purchase permitted");
        }


        #region Private Methods
        private async Task<CustomerList> GetListWihtPagination(int? page, int pageSize)
        {
            List<Customer> customers = new List<Customer>();

            customers = await _ctx.Customers
                        .Include(c => c.Orders)
                        .OrderBy(r => r.Id)
                        .Skip((page.Value - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToListAsync();

            int totalItems = await _ctx.Customers.CountAsync();
            pageSize = customers.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            bool hasNextPage = page < totalPages;

            return new CustomerList(customers, pageSize, hasNextPage);
        }
        private async Task<CustomerList> GetListWithoutPagination()
        {
            List<Customer> customers = new List<Customer>();

            customers = await _ctx.Customers
                       .Include(c => c.Orders)
                       .OrderBy(r => r.Id)
                       .AsNoTracking()
                       .ToListAsync();

            int pageSize = customers.Count;

            return new CustomerList(customers, pageSize, false);
        }
        #endregion
    }
}
