using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public class AddCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                              IMapper mapper) : IRequestHandler<AddCustomerAddressRequest, AddCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;

    public async Task<AddCustomerAddressResponse> Handle(AddCustomerAddressRequest addCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = _mapper.Map<Domain.CustomerAddress>(addCustomerAddressRequest); 
        await _customerAddressRepository.AddAsync(customerAddress);

        return new AddCustomerAddressResponse("Customer Added.");
    }
}