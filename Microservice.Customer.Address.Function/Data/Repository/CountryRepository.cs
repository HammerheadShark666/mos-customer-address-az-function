using Microservice.Customer.Address.Function.Data.Context;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Repository;

public class CountryRepository(CustomerAddressDbContext context) : ICountryRepository
{
    private readonly CustomerAddressDbContext _context = context;

    public async Task<bool> ExistsAsync(int countryId)
    {
        return await _context.Country.AnyAsync(x => x.Id.Equals(countryId));
    }
}