using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class CustomerDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomerTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Customer GetSampleCustomer(int id = 1, string email = "customer@email.com")
    {
        return new Customer
        {
            Id = id,
            Name = "Test Customer",
            Email = email,
            Phone = "1234567890",
            Address = "Test Address",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsCustomer()
    {
        var context = GetInMemoryDbContext();
        var service = new CustomerDataService(context);
        var customer = GetSampleCustomer();

        await service.Add(customer);

        Assert.Single(context.Customers);
        Assert.Equal("Test Customer", context.Customers.First().Name);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCustomers()
    {
        var context = GetInMemoryDbContext();
        context.Customers.Add(GetSampleCustomer(1, "a@email.com"));
        context.Customers.Add(GetSampleCustomer(2, "b@email.com"));
        context.SaveChanges();

        var service = new CustomerDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectCustomer()
    {
        var context = GetInMemoryDbContext();
        context.Customers.Add(GetSampleCustomer(1, "a@email.com"));
        context.Customers.Add(GetSampleCustomer(2, "b@email.com"));
        context.SaveChanges();

        var service = new CustomerDataService(context);

        var customer = await service.GetByKey(2);

        Assert.NotNull(customer);
        Assert.Equal("b@email.com", customer.Email);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfCustomerExists()
    {
        var context = GetInMemoryDbContext();
        context.Customers.Add(GetSampleCustomer(1));
        context.SaveChanges();

        var service = new CustomerDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfCustomerDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CustomerDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesCustomer()
    {
        var context = GetInMemoryDbContext();
        var customer = GetSampleCustomer(1, "a@email.com");
        context.Customers.Add(customer);
        context.SaveChanges();

        var service = new CustomerDataService(context);

        customer.Name = "Updated Name";
        await service.Update(customer);

        var updated = context.Customers.First(c => c.Id == 1);
        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task Update_ThrowsIfCustomerDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CustomerDataService(context);
        var customer = GetSampleCustomer(99, "noexiste@email.com");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(customer));
    }

    [Fact]
    public async Task Remove_RemovesCustomer()
    {
        var context = GetInMemoryDbContext();
        var customer = GetSampleCustomer(1, "a@email.com");
        context.Customers.Add(customer);
        context.SaveChanges();

        var service = new CustomerDataService(context);

        await service.Remove(customer);

        Assert.Empty(context.Customers);
    }

    [Fact]
    public async Task Remove_ThrowsIfCustomerDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CustomerDataService(context);
        var customer = GetSampleCustomer(99, "noexiste@email.com");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(customer));
    }

    [Fact]
    public async Task AddMultiple_AddsAllCustomers()
    {
        var context = GetInMemoryDbContext();
        var service = new CustomerDataService(context);
        var customers = new List<Customer>
        {
            GetSampleCustomer(1, "a@email.com"),
            GetSampleCustomer(2, "b@email.com")
        };

        await service.AddMultiple(customers);

        Assert.Equal(2, context.Customers.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllCustomers()
    {
        var context = GetInMemoryDbContext();
        var customers = new List<Customer>
        {
            GetSampleCustomer(1, "a@email.com"),
            GetSampleCustomer(2, "b@email.com")
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();

        var service = new CustomerDataService(context);

        customers[0].Name = "Updated A";
        customers[1].Name = "Updated B";
        await service.UpdateMultiple(customers);

        Assert.Equal("Updated A", context.Customers.First(c => c.Id == 1).Name);
        Assert.Equal("Updated B", context.Customers.First(c => c.Id == 2).Name);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllCustomers()
    {
        var context = GetInMemoryDbContext();
        var customers = new List<Customer>
        {
            GetSampleCustomer(1, "a@email.com"),
            GetSampleCustomer(2, "b@email.com")
        };
        context.Customers.AddRange(customers);
        context.SaveChanges();

        var service = new CustomerDataService(context);

        await service.RemoveMultiple(customers);

        Assert.Empty(context.Customers);
    }
}