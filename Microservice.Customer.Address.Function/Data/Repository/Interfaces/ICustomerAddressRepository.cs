namespace Microservice.Customer.Address.Function.Data.Repository.Interfaces;

public interface ICustomerAddressRepository
{
    Task<Domain.CustomerAddress> AddAsync(Domain.CustomerAddress customerAddress);
}