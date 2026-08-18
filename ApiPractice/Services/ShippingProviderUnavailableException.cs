namespace ApiPractice.Services;

public class ShippingProviderUnavailableException : Exception
{
    public ShippingProviderUnavailableException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
