using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Contracts;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Application.Services
{
    public class PurchaseService : IPurchaseService
    {

        private readonly TestDbContext _ctx;

        public PurchaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> CustumerExists(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(customerId));

            return await _ctx.Customers.AnyAsync(x => x.Id.Equals(customerId));
        }

        public async Task<bool> CanPurchaseThisMonth(int customerId)
        {
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders
                .CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            return ordersInThisMonth == 0;
        }

        public async Task<bool> IsFirstPurchaseValid(int customerId, decimal purchaseValue)
        {
            if (purchaseValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            var hasBoughtBefore = await _ctx.Orders.AnyAsync(s => s.CustomerId == customerId);

            if (!hasBoughtBefore && purchaseValue > 100)
                return false;

            return true;

        }

        public bool IsBusinessHours(bool bypassBusinessHours = false)
        {
            if (bypassBusinessHours)
                return true;

            var now = DateTime.UtcNow;
            if (now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }

    }
}
