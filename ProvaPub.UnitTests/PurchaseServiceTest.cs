using Microsoft.EntityFrameworkCore;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.UnitTests
{
    [TestClass]
    public sealed class PurchaseServiceTest
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
        public async Task CustumerExists_ShouldTrue_WhenCustomerExist()
        {
            int fakeCustomerId = 1;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);

            var custumerExists = await _purchaseService.CustumerExists(fakeCustomerId);
            Assert.IsTrue(custumerExists);
        }

        [TestMethod]
        public async Task CustumerExists_ShouldThrow_WhenCustomerDoesNotExist()
        {
            int fakeCustomerId = 999;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);

            var custumerExists = await _purchaseService.CustumerExists(fakeCustomerId);
            Assert.IsFalse(custumerExists);
        }

        [TestMethod]
        public async Task CustumerExists_ShouldThrow_WhenCustomerIdLessThanZero()
        {
            int fakeCustomerId = -1;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _purchaseService.CustumerExists(fakeCustomerId));
        }

        [TestMethod]
        public async Task CustumerExists_ShouldThrow_WhenCustomerIdEqualsToZero()
        {
            int fakeCustomerId = 0;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);

            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _purchaseService.CustumerExists(fakeCustomerId));
        }

        [TestMethod]
        public async Task CanPurchaseThisMonth_ShouldFalse_WhenMoreThanOnePurchasePerMonth()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 150;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);

            await _ctx.Orders.AddAsync(new Order(purchaseValue, fakeCustomerId, DateTime.UtcNow));
            await _ctx.SaveChangesAsync();

            var canPurchase = await _purchaseService.CanPurchaseThisMonth(fakeCustomerId);
            Assert.IsFalse(canPurchase);
        }

        [TestMethod]
        public async Task CanPurchaseThisMonth_ShouldTrue_WhenWithoutPurchaseAtMonth()
        {
            int fakeCustomerId = 1;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var canPurchase = await _purchaseService.CanPurchaseThisMonth(fakeCustomerId);
            Assert.IsTrue(canPurchase);
        }

        [TestMethod]
        public async Task IsFirstPurchaseValid_ShouldFalse_WhenPurchaseValueMoreThanPermitted()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 150;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var canPurchase = await _purchaseService.IsFirstPurchaseValid(fakeCustomerId, purchaseValue);
            Assert.IsFalse(canPurchase);
        }

        [TestMethod]
        public async Task IsFirstPurchaseValid_ShouldFalse_WhenPurchaseValueLessThanZero()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = -50;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _purchaseService.IsFirstPurchaseValid(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task IsFirstPurchaseValid_ShouldFalse_WhenPurchaseValueEqualsZero()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 0;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await _purchaseService.IsFirstPurchaseValid(fakeCustomerId, purchaseValue));
        }

        [TestMethod]
        public async Task IsFirstPurchaseValid_ShouldTrue_WhenPurchaseValuePermitted()
        {
            int fakeCustomerId = 1;
            decimal purchaseValue = 50;
            var _ctx = GetInMemoryDbContext();
            var _purchaseService = new PurchaseService(_ctx);
            var canPurchase = await _purchaseService.IsFirstPurchaseValid(fakeCustomerId, purchaseValue);
            Assert.IsTrue(canPurchase);
        }
    }
}
