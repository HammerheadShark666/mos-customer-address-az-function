namespace Microservice.Customer.Address.Function.Helpers;

public class Constants
{ 
    public const string DatabaseConnectionString = "SQLAZURECONNSTR_CUSTOMER_ADDRESS";
     
    public const string AzureServiceBusConnection = "AZURE_SERVICE_BUS_CONNECTION";

    public const string RegisteredUserCustomerAddressSBQueue = "registered-user-customer-address";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";
}
