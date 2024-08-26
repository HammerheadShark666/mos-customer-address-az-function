using Microservice.Customer.Address.Function.Data.Configuration;
using Microservice.Customer.Address.Function.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Context;
public class CustomerAddressDbContext(DbContextOptions<CustomerAddressDbContext> options) : DbContext(options)
{
    public DbSet<CustomerAddress> CustomerAddress { get; set; }
    public DbSet<Country> Country { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration());
    }
}