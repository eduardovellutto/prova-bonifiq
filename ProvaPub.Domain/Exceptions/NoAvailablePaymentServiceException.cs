namespace ProvaPub.Domain.Exceptions
{
    public class NoAvailablePaymentServiceException : Exception
    {
        public NoAvailablePaymentServiceException(string message) : base(message) { }
    }

}
