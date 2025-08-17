using ProvaPub.Domain.Models.Entities;

namespace ProvaPub.Domain.Contracts
{
    public interface IOrderService
    {
        Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId);
    }
}
