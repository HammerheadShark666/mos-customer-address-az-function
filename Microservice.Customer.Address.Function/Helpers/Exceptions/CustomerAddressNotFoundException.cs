namespace Microservice.Customer.AddressFunction.Helpers.Exceptions;

public class CustomerAddressNotFoundException : Exception
{
    public CustomerAddressNotFoundException()
    {
    }

    public CustomerAddressNotFoundException(string message)
        : base(message)
    {
    }

    public CustomerAddressNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}