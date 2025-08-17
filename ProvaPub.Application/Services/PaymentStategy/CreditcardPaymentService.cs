using ProvaPub.Domain.Contracts;

namespace ProvaPub.Application.Services.PaymentStategy
{
    public class CreditcardPaymentService : IPaymentService
    {
        public string PaymentMethod => "creditcard";

        public async Task PayAsync(decimal amount, int customerId)
        {
            // Lógica de pagamento via Pix
            await Task.CompletedTask;
        }
    }
}
