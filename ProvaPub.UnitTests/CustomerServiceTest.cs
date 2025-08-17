using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.UnitTests
{
    [TestClass]
    public sealed class CustomerServiceTest
    {
        private TestDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TestDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [TestMethod]
        public async Task CanPurchase_ShouldThrow_WhenCustomerDoesNotExist()
        {
            int fakeCustomerId = 999;
            decimal purchaseValue = 50;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await _customerService.CanPurchase(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task CanPurchase_ShouldThrow_WhenCustomerIdLessThanZero()
        {
            int fakeCustomerId = -1;
            decimal purchaseValue = 50;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _customerService.CanPurchase(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task CanPurchase_ShouldThrow_WhenCustomerIdEqualsToZero()
        {
            int fakeCustomerId = 0;
            decimal purchaseValue = 50;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _customerService.CanPurchase(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task CanPurchase_ShouldFalse_WhenMoreThanOnePurchasePerMonth()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 50;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await _ctx.Orders.AddAsync(new Order(purchaseValue, fakeCustomerId, DateTime.UtcNow));
            await _ctx.SaveChangesAsync();

            var canPurchase = await _customerService.CanPurchase(fakeCustomerId, purchaseValue);
            Assert.IsFalse(canPurchase.CanPurchase);
        }



        [TestMethod]
        public async Task CanPurchase_ShouldFalse_WhenIsFirstPurchaseWithValueMoreThanPermitted()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 150;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            var canPurchase = await _customerService.CanPurchase(fakeCustomerId, purchaseValue);
            Assert.IsFalse(canPurchase.CanPurchase);
        }

        [TestMethod]
        public async Task CanPurchase_ShouldTrue_WhenIsFirstPurchaseWithValuePermitted()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 50;
            bool bypassBusinessHour = true;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            var canPurchase = await _customerService.CanPurchase(fakeCustomerId, purchaseValue, bypassBusinessHour);
            Assert.IsTrue(canPurchase.CanPurchase);
        }

        [TestMethod]
        public async Task CanPurchase_ShouldTrue_WhenIsFirstPurchaseWithValueLessThanZero()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = -150;
            bool bypassBusinessHour = true;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _customerService.CanPurchase(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task CanPurchase_ShouldTrue_WhenIsFirstPurchaseWithValueEqualsToZero()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 0;
            bool bypassBusinessHour = true;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _customerService.CanPurchase(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task CanPurchase_ShouldFalse_WhenIsOverBusinessHours()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 150;

            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var _customerService = new CustomerService(_ctx, _purchaseService);

            var canPurchase = await _customerService.CanPurchase(fakeCustomerId, purchaseValue);
            Assert.IsFalse(canPurchase.CanPurchase);
        }
    }
}
