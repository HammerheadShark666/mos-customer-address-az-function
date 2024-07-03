using Microservice.Customer.Address.Function.Data.Contexts;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly CustomerAddressDbContext _context;

    public CountryRepository(CustomerAddressDbContext context) 
    {
        _context = context;
    } 

    public async Task<bool> ExistsAsync(int countryId)
    {
        return await _context.Country.AnyAsync(x => x.Id.Equals(countryId));
    }
}