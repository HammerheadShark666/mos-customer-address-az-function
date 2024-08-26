using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Function.Data.Context;
using Microservice.Customer.Address.Function.Data.Repository;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microservice.Customer.Address.Function.Helpers;
using Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddValidatorsFromAssemblyContaining<AddCustomerAddressValidator>();
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerAddressMapper)));
        services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddMemoryCache();

        services.AddDbContextFactory<CustomerAddressDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));
    })
    .Build();

await host.RunAsync();
