namespace ProvaPub.Domain.Contracts
{
    public interface IPaymentService
    {
        string PaymentMethod { get; }
        Task PayAsync(decimal amount, int customerId);
    }
}
