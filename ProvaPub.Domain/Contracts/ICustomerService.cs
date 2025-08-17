using ProvaPub.Domain.Models.Entities;
using ProvaPub.Domain.Models.Responses;

namespace ProvaPub.Domain.Contracts
{
    public interface ICustomerService : IServiceBase<CustomerList>
    {
        Task<PurchaseResponse> CanPurchase(int customerId, decimal purchaseValue, bool bypassBusinessHour = false);
    }
}
