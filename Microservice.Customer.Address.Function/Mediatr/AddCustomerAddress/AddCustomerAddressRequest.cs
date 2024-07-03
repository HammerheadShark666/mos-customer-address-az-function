using MediatR;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public record AddCustomerAddressRequest(Guid Id, Guid CustomerId, string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId) : IRequest<AddCustomerAddressResponse>;

//public record AddCustomerAddress(string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId);