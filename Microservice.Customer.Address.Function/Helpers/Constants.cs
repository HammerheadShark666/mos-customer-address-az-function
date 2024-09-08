namespace Microservice.Customer.Address.Function.Helpers;

public class Constants
{
    public const string DatabaseConnectionString = "SQLAZURECONNSTR_CUSTOMER_ADDRESS";

    public const string AzureServiceBusConnection = "ServiceBusConnection";
    public const string AzureServiceBusQueueRegisteredUserCustomerAddress = "AZURE_SERVICE_BUS_QUEUE_CUSTOMER_ADDRESS";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";
}