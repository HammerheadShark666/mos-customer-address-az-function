using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Address.Function.Helpers;
using Microservice.Customer.Address.Function.Helpers.Exceptions;
using Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Customer.Address.Function.Functions;

public class AddCustomerAddressFromRegisteredUser(ILogger<AddCustomerAddressFromRegisteredUser> logger, IMediator mediator)
{
    [Function(nameof(AddCustomerAddressFromRegisteredUser))]
    public async Task Run([ServiceBusTrigger("%" + Constants.AzureServiceBusQueueRegisteredUserCustomerAddress + "%",
                                             Connection = Constants.AzureServiceBusConnectionManagedIdentity)]
                                             ServiceBusReceivedMessage message,
                                             ServiceBusMessageActions messageActions)
    {
        AddCustomerAddressRequest? addCustomerAddressRequest = JsonHelper.GetRequest<AddCustomerAddressRequest>(message.Body.ToArray()) ?? throw new JsonDeserializeException("Error deserializing AddCustomerAddressRequest.");

        try
        {
            logger.LogInformation("RegisteredUser - AddCustomerAddress - {addCustomerAddressRequest.CustomerId}.", addCustomerAddressRequest.CustomerId);

            await mediator.Send(addCustomerAddressRequest);
            await messageActions.CompleteMessageAsync(message);

            return;
        }
        catch (JsonDeserializeException jsonDeserializeException)
        {
            logger.LogError("RegisteredUser - Error deserializing AddCustomerAddressRequest. {message}", message);
            await messageActions.DeadLetterMessageAsync(message, null, jsonDeserializeException.Message);
        }
        catch (FluentValidation.ValidationException validationException)
        {
            logger.LogError("Validation Failures: Id: {addCustomerAddressRequest.Id}", addCustomerAddressRequest.Id);
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {
            logger.LogError("Internal Error: Id: {addCustomerAddressRequest.Id}", addCustomerAddressRequest.Id);
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonInternal, ex.Message);
        }
    }
}