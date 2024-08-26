namespace Microservice.Customer.Address.Function.Data.Repository.Interfaces;

public interface ICountryRepository
{
    Task<bool> ExistsAsync(int countryId);
}