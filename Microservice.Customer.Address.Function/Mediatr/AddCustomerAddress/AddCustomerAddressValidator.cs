using FluentValidation; 
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;

public class AddCustomerAddressValidator : AbstractValidator<AddCustomerAddressRequest>
{
    private readonly ICountryRepository _countryRepository;

    public AddCustomerAddressValidator(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;

        RuleFor(address => address.AddressLine1)
                   .NotEmpty().WithMessage("Address line 1 is required.")
                   .Length(1, 50).WithMessage("Address line 1 length is between 1 and 50.");

        RuleFor(address => address.TownCity)
              .NotEmpty().WithMessage("Town/City is required.")
              .Length(1, 50).WithMessage("Town/City length is between 1 and 50.");

        RuleFor(address => address.County) 
              .MaximumLength(50).WithMessage("County length is between 1 and 50."); 

        RuleFor(address => address.Postcode)
              .NotEmpty().WithMessage("Postcode is required.")
              .Length(6, 8).WithMessage("Postcode length is between 6 and 8.");

        RuleFor(address => address).MustAsync(async (address, cancellation) =>
        {
            return await CountryExists(address.CountryId);
        })
        .WithMessage(x => $"The country does not exist."); 
    }

    protected async Task<bool> CountryExists(int id)
    {
        return await _countryRepository.ExistsAsync(id);
    } 
}