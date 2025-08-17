namespace ProvaPub.Domain.Exceptions
{
    public class NoAvailableCustomersException : Exception
    {
        public NoAvailableCustomersException(string message) : base(message) { }
    }

}
