namespace ProvaPub.Domain.Exceptions
{
    public class NoAvailableProductsException : Exception
    {
        public NoAvailableProductsException(string message) : base(message) { }
    }

}
