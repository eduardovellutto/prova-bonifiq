namespace ProvaPub.Domain.Contracts
{
    public interface IPurchaseService
    {
        Task<bool> CustumerExists(int customerId);
        Task<bool> CanPurchaseThisMonth(int customerId);
        Task<bool> IsFirstPurchaseValid(int customerId, decimal purchaseValue);
        bool IsBusinessHours(bool bypassBusinessHours = false);
    }
}
