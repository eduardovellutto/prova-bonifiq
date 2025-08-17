namespace ProvaPub.Domain.Models.Responses
{
    public class PurchaseResponse
    {
        #region Properties
        public bool CanPurchase { get; private set; }
        public string Message { get; private set; } = string.Empty;
        #endregion

        #region Constructors
        public PurchaseResponse(bool canPurchase, string message)
        {
            CanPurchase = canPurchase;
            Message = message;
        }
        #endregion
    }
}
