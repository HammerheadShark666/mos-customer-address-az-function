using Microservice.Customer.Address.Function.Data.Context;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Repository;

public class CountryRepository(CustomerAddressDbContext context) : ICountryRepository
{
    public async Task<bool> ExistsAsync(int countryId)
    {
        return await context.Country.AnyAsync(x => x.Id.Equals(countryId));
    }
}