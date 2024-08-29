using Microservice.Customer.Address.Function.Data.Context;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Repository;

public class CustomerAddressRepository(IDbContextFactory<CustomerAddressDbContext> dbContextFactory) : ICustomerAddressRepository
{
    public async Task<Domain.CustomerAddress> AddAsync(Domain.CustomerAddress customerAddress)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.CustomerAddress.AddAsync(customerAddress);
        db.SaveChanges();

        return customerAddress;
    }
}