using Microservice.Customer.Address.Function.Data.Configuration;
using Microservice.Customer.Address.Function.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Function.Data.Contexts;
public class CustomerAddressDbContext : DbContext
{ 
    public CustomerAddressDbContext(DbContextOptions<CustomerAddressDbContext> options) : base(options) { }
 
    public DbSet<CustomerAddress> CustomerAddress { get; set; }
    public DbSet<Country> Country { get; set; }      

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration()); 
    }
}