using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Address.Function.Functions;
using Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Microservice.Customer.Function.Test.Unit;

public class AddCustomerAddressFromRegisteredUserAzureFunctionTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<AddCustomerAddressFromRegisteredUser>> _mockLogger;
    private readonly AddCustomerAddressFromRegisteredUser _addCustomerAddressFromRegisteredUser; 
    
    public AddCustomerAddressFromRegisteredUserAzureFunctionTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<AddCustomerAddressFromRegisteredUser>>();
        _addCustomerAddressFromRegisteredUser = new AddCustomerAddressFromRegisteredUser(_mockLogger.Object, _mockMediator.Object);
    }     

    [Test]
    public async Task Azure_function_trigger_service_bus_recieve_return_succeed()
    {
        var addCustomerAddressRequest = new AddCustomerAddressRequest(Guid.NewGuid(), Guid.NewGuid(), "AddressLine1", "AddressLine2", "AddressLine3",
                                                                                   "TownCity1", "County1", "Pcode1", 1);

        var mockMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromString(JsonConvert.SerializeObject(addCustomerAddressRequest)), correlationId: Guid.NewGuid().ToString());

        var mockServiceBusMessageActions = new Mock<ServiceBusMessageActions>();
        mockServiceBusMessageActions.Setup(x => x.CompleteMessageAsync(mockMessage, CancellationToken.None)).Returns(Task.FromResult(true));
          
        await _addCustomerAddressFromRegisteredUser.Run(mockMessage, mockServiceBusMessageActions.Object);        
    } 
}