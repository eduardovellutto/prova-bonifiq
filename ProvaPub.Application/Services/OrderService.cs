using Microsoft.EntityFrameworkCore;
using ProvaPub.Domain.Contracts;
using ProvaPub.Domain.Exceptions;
using ProvaPub.Domain.Models.Entities;
using ProvaPub.Infrastructure.Repository;

namespace ProvaPub.Application.Services
{
    public class OrderService : IOrderService
    {
        TestDbContext _ctx;
        private readonly IEnumerable<IPaymentService> _paymentServices;

        public OrderService(TestDbContext ctx, IEnumerable<IPaymentService> paymentServices)
        {
            _ctx = ctx;
            _paymentServices = paymentServices;
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            bool customerExists = await _ctx.Customers.AsNoTracking().AnyAsync(x => x.Id.Equals(customerId));

            if (!customerExists)
                throw new NoAvailableCustomersException("Custumer não encontrado");

            var paymentService = GetPaymentService(paymentMethod);

            await paymentService.PayAsync(paymentValue, customerId);

            Order order = new Order(paymentValue, customerId, DateTime.UtcNow);
            var oderAdd = await InsertOrder(order);

            return oderAdd;
        }


        #region Private Methods
        private async Task<Order> InsertOrder(Order order)
        {
            var orderCreated = (await _ctx.Orders.AddAsync(order)).Entity;
            await _ctx.SaveChangesAsync();
            return orderCreated;
        }
        private IPaymentService GetPaymentService(string paymentMethod)
        {
            var paymentService = _paymentServices
                .FirstOrDefault(p => p.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

            if (paymentService == null)
                throw new NoAvailablePaymentServiceException($"Método de pagamento '{paymentMethod}' não suportado.");

            return paymentService;
        }

        #endregion
    }
}
