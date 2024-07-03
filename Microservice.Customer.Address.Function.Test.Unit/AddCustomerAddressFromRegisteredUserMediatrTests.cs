using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Function.Data.Repository.Interfaces;
using Microservice.Customer.Address.Function.Helpers;
using Microservice.Customer.Address.Function.MediatR.AddCustomerAddress;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Function.Test.Unit;

[TestFixture]
public class AddCustomerAddressFromRegisteredUserMediatrTests
{
    private Mock<ICustomerAddressRepository> customerAddressRepositoryMock = new Mock<ICustomerAddressRepository>();
    private Mock<ICountryRepository> countryRepositoryMock = new Mock<ICountryRepository>();
    private ServiceCollection services = new ServiceCollection();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerAddressValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddCustomerAddressCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerAddressRepository>(sp => customerAddressRepositoryMock.Object);
        services.AddScoped<ICountryRepository>(sp => countryRepositoryMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerAddressMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Customer_added_return_success_message()
    {
        var customerAddress = new Address.Function.Domain.CustomerAddress() { 
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            AddressLine1 = "AddressLine1",
            AddressLine2 = "AddressLine2",
            AddressLine3 = "AddressLine3",
            TownCity = "TownCity1",
            County = "County1",
            Postcode = "Pcode1",
            CountryId = 1
        };         

        customerAddressRepositoryMock
                .Setup(x => x.AddAsync(customerAddress))
                .Returns(Task.FromResult(customerAddress));

        countryRepositoryMock
                .Setup(x => x.ExistsAsync(1))
                .Returns(Task.FromResult(true));

        var addCustomerRequest = new AddCustomerAddressRequest(Guid.NewGuid(), Guid.NewGuid(), 
                                                                    "AddressLine1", "AddressLine2", "AddressLine3",
                                                                        "TownCity1", "County1", "Pcode1", 1);

        var actualResult = await mediator.Send(addCustomerRequest);
        var expectedResult = "Customer Added.";

        Assert.That(actualResult.message, Is.EqualTo(expectedResult));
    }

    [Test]
    public void User_not_added_no_address_return_exception_fail_message()
    {
        countryRepositoryMock
                .Setup(x => x.ExistsAsync(0))
                .Returns(Task.FromResult(false));


        var addCustomerAddressRequest = new AddCustomerAddressRequest(Guid.NewGuid(), Guid.NewGuid(),
                                                                    "", "AddressLine2", "AddressLine3",
                                                                        "", "", "", 0);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerAddressRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(7));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Address line 1 is required."));
        Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Address line 1 length is between 1 and 50."));
        Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("Town/City is required."));
        Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Town/City length is between 1 and 50.")); 
        Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("Postcode is required."));
        Assert.That(validationException.Errors.ElementAt(5).ErrorMessage, Is.EqualTo("Postcode length is between 6 and 8.")); 
        Assert.That(validationException.Errors.ElementAt(6).ErrorMessage, Is.EqualTo("The country does not exist."));
    }

    [Test]
    public void User_not_added_invalid_address_return_exception_fail_message()
    {
        countryRepositoryMock
                .Setup(x => x.ExistsAsync(0))
                .Returns(Task.FromResult(false));

        var addCustomerAddressRequest = new AddCustomerAddressRequest(Guid.NewGuid(), Guid.NewGuid(), 
                                                                      "AddressLine1AddressLine1AddressLine1AddressLine1AddressLine1",
                                                                      "AddressLine2", "AddressLine3",
                                                                      "TownCityTownCityTownCityTownCityTownCityTownCityTownCityTownCity",
                                                                      "CountyCountyCountyCountyCountyCountyCountyCountyCountyCountyCountyCountyCountyCounty",
                                                                      "PostcodePostcodePostcodePostcodePostcodePostcodePostcode", 16);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerAddressRequest);
        });

        Assert.That(validationException.Errors.Count, Is.EqualTo(5));
        Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Address line 1 length is between 1 and 50."));
        Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Town/City length is between 1 and 50."));
        Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("County length is between 1 and 50."));
        Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Postcode length is between 6 and 8."));
        Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("The country does not exist."));
    }










    //[Test]
    //public void Customer_not_added_id_exists_return_exception_fail_message()
    //{
    //    var customerId = Guid.NewGuid();

    //    customerRepositoryMock
    //            .Setup(x => x.CustomerExistsAsync(customerId))
    //            .Returns(Task.FromResult(true));

    //    var command = new AddCustomerRequest(customerId, "ValidEmail@hotmail.com", "TestSurname", "TestFirstName");

    //    var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
    //    {
    //        await mediator.Send(command);
    //    });

    //    Assert.That(validationException.Errors.Count, Is.EqualTo(1));
    //    Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Customer with this id already exists"));
    //}

    //[Test]
    //public void Customer_not_added_email_exists_return_exception_fail_message()
    //{   
    //    customerRepositoryMock
    //            .Setup(x => x.CustomerExistsAsync("InvalidEmail@hotmail.com"))
    //            .Returns(Task.FromResult(true));  

    //    var command = new AddCustomerRequest(Guid.NewGuid(), "InvalidEmail@hotmail.com", "TestSurname", "TestFirstName");

    //    var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
    //    {
    //        await mediator.Send(command);
    //    });

    //    Assert.That(validationException .Errors.Count, Is.EqualTo(1));
    //    Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Customer with this email already exists")); 
    //}


    //[Test]
    //public void Customer_not_added_invalid_email_return_exception_fail_message()
    //{
    //    customerRepositoryMock
    //            .Setup(x => x.CustomerExistsAsync("InvalidEmail"))
    //            .Returns(Task.FromResult(false));

    //    var command = new AddCustomerRequest(Guid.NewGuid(), "InvalidEmail", "TestSurname", "TestFirstName");

    //    var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
    //    {
    //        await mediator.Send(command);
    //    });

    //    Assert.That(validationException.Errors.Count, Is.EqualTo(1));
    //    Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Invalid Email."));
    //}

    //[Test]
    //public void Customer_not_added_invalid_surname_firstname_return_exception_fail_message()
    //{
    //    customerRepositoryMock
    //            .Setup(x => x.CustomerExistsAsync("ValidEmail@hotmail.com"))
    //            .Returns(Task.FromResult(false));

    //    var command = new AddCustomerRequest(Guid.NewGuid(), "ValidEmail@hotmail.com", "TestSurnameTestSurnameTestSurnameTestSurnameTestSurnameTestSurname", "TestFirstNameTestFirstNameTestFirstNameTestFirstNameTestFirstName");

    //    var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
    //    {
    //        await mediator.Send(command);
    //    });

    //    Assert.That(validationException.Errors.Count, Is.EqualTo(2));
    //    Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Surname length between 1 and 30."));
    //    Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("First name length between 1 and 30."));
    //}

    //[Test]
    //public void Customer_not_added_no_email_surname_firstname_return_exception_fail_message()
    //{
    //    customerRepositoryMock
    //            .Setup(x => x.CustomerExistsAsync("ValidEmail@hotmail.com"))
    //            .Returns(Task.FromResult(false));

    //    var command = new AddCustomerRequest(Guid.NewGuid(), "", "", "");

    //    var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
    //    {
    //        await mediator.Send(command);
    //    });

    //    Assert.That(validationException.Errors.Count, Is.EqualTo(7));
    //    Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Email is required."));
    //    Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Email length between 8 and 150."));
    //    Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("Invalid Email."));    
    //    Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Surname is required."));
    //    Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("Surname length between 1 and 30."));
    //    Assert.That(validationException.Errors.ElementAt(5).ErrorMessage, Is.EqualTo("First name is required."));
    //    Assert.That(validationException.Errors.ElementAt(6).ErrorMessage, Is.EqualTo("First name length between 1 and 30."));
    //} 
}