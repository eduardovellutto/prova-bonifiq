using ProvaPub.Domain.Contracts;

namespace ProvaPub.Application.Services.PaymentStategy
{
    public class PixPaymentService : IPaymentService
    {
        public string PaymentMethod => "pix";

        public async Task PayAsync(decimal amount, int customerId)
        {
            // Lógica de pagamento via Pix
            await Task.CompletedTask;
        }
    }
}
