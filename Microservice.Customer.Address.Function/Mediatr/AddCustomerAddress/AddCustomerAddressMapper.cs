using AutoMapper;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public class AddCustomerAddressMapper : Profile
{
    public AddCustomerAddressMapper()
    {
        base.CreateMap<AddCustomerAddressRequest, Microservice.Customer.Address.Function.Domain.CustomerAddress>();
    }
}