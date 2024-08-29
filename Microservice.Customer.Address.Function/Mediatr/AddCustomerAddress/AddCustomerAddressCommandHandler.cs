using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public class AddCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                              IMapper mapper) : IRequestHandler<AddCustomerAddressRequest, AddCustomerAddressResponse>
{
    public async Task<AddCustomerAddressResponse> Handle(AddCustomerAddressRequest addCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = mapper.Map<Domain.CustomerAddress>(addCustomerAddressRequest);
        await customerAddressRepository.AddAsync(customerAddress);

        return new AddCustomerAddressResponse("Customer Added.");
    }
}