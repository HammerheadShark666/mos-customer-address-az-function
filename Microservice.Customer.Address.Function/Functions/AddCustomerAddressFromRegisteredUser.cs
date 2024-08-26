using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Address.Function.Helpers;
using Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Customer.Address.Function.Functions;

public class AddCustomerAddressFromRegisteredUser(ILogger<AddCustomerAddressFromRegisteredUser> logger, IMediator mediator)
{
    private ILogger<AddCustomerAddressFromRegisteredUser> _logger { get; set; } = logger;
    private IMediator _mediator { get; set; } = mediator;


    [Function(nameof(AddCustomerAddressFromRegisteredUser))]
    public async Task Run([ServiceBusTrigger("%" + Constants.AzureServiceBusQueueRegisteredUserCustomerAddress + "%",
                                             Connection = Constants.AzureServiceBusConnection)]
                                             ServiceBusReceivedMessage message,
                                             ServiceBusMessageActions messageActions)
    {
        var addCustomerAddressRequest = JsonHelper.GetRequest<AddCustomerAddressRequest>(message.Body.ToArray());

        try
        {
            _logger.LogInformation($"RegisteredUser - AddCustomerAddress - {addCustomerAddressRequest.Id}.");

            await _mediator.Send(addCustomerAddressRequest);
            await messageActions.CompleteMessageAsync(message);

            return;
        }
        catch (FluentValidation.ValidationException validationException)
        {
            _logger.LogError($"Validation Failures: Id: {addCustomerAddressRequest.Id}");
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Internal Error: Id: {addCustomerAddressRequest.Id}");
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonInternal, ex.Message);
        }
    }
}