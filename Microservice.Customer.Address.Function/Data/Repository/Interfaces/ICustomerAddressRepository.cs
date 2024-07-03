namespace Microservice.Customer.Address.Function.Data.Repository.Interfaces;

public interface ICustomerAddressRepository
{
    Task<Domain.CustomerAddress> AddAsync(Domain.CustomerAddress customerAddress);
    Task Update(Domain.CustomerAddress customerAddress);
    Task Delete(Domain.CustomerAddress customerAddress);
    Task<List<Domain.CustomerAddress>> ByCustomerAsync(Guid customerId);
    Task<Domain.CustomerAddress> ByIdAsync(Guid customerId, Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> HasAddressesAsync(Guid customerId);
}