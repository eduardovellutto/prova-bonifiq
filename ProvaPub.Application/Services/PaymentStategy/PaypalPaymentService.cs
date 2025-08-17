using ProvaPub.Domain.Contracts;

namespace ProvaPub.Application.Services.PaymentStategy
{
    public class PaypalPaymentService : IPaymentService
    {
        public string PaymentMethod => "paypal";

        public async Task PayAsync(decimal amount, int customerId)
        {
            // Lógica de pagamento via Pix
            await Task.CompletedTask;
        }
    }
}
