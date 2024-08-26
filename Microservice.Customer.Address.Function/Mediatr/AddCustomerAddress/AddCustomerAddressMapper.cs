using AutoMapper;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public class AddCustomerAddressMapper : Profile
{
    public AddCustomerAddressMapper()
    {
        base.CreateMap<AddCustomerAddressRequest, Microservice.Customer.Address.Function.Domain.CustomerAddress>();
        //.ForMember(dest => dest.Id, opt => opt.Ignore())
        //.ForMember(x => x.CustomerId, act => act.MapFrom(src => src.Id))
        //.ForMember(x => x.AddressLine1, act => act.MapFrom(src => src.Address.AddressLine1))
        //.ForMember(x => x.AddressLine2, act => act.MapFrom(src => src.Address.AddressLine2))
        //.ForMember(x => x.AddressLine3, act => act.MapFrom(src => src.Address.AddressLine3))
        //.ForMember(x => x.TownCity, act => act.MapFrom(src => src.Address.TownCity))
        //.ForMember(x => x.CountryId, act => act.MapFrom(src => src.Address.CountryId))
        //.ForMember(x => x.County, act => act.MapFrom(src => src.Address.County))
        //.ForMember(x => x.Postcode, act => act.MapFrom(src => src.Address.Postcode));
    }
}