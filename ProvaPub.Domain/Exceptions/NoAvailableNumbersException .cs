namespace ProvaPub.Domain.Exceptions
{
    public class NoAvailableNumbersException : Exception
    {
        public NoAvailableNumbersException(string message) : base(message) { }
    }

}
