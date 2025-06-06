using API.DbContexts;
using API.DataServices;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace API.Tests.DataServices
{
    public class CustomerDataServiceTests
    {
        private ApplicationDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private Customer CreateValidCustomer(int id, string name)
        {
            return new Customer
            {
                Id = id,
                Name = name,
                Address = "Calle Ficticia 123",
                Phone = "123456789",
                Email = "email@prueba.com",
                CreatedBy = "test",
                ModifiedBy = "test"
            };
        }

        [Fact]
        public async Task Add_Should_Add_Customer_To_Database()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = GetInMemoryDbContext(dbName);
            var service = new CustomerDataService(context);

            var customer = CreateValidCustomer(1, "Juan Pérez");

            await service.Add(customer);
            var saved = await context.Customers.FindAsync(1);

            Assert.NotNull(saved);
            Assert.Equal("Juan Pérez", saved.Name);
        }

[Fact]
public async Task GetByKey_Should_Return_Null_If_Customer_Does_Not_Exist()
{
    var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
    var service = new CustomerDataService(context);

    var result = await service.GetByKey(999);

    Assert.Null(result);
}

        [Fact]
        public async Task GetByKey_Should_Return_Customer_If_Exists()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = GetInMemoryDbContext(dbName);
            var service = new CustomerDataService(context);

            var customer = CreateValidCustomer(2, "Ana");
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var result = await service.GetByKey(2);

            Assert.NotNull(result);
            Assert.Equal("Ana", result!.Name);
        }

        [Fact]
        public async Task Exists_Should_Return_True_If_Customer_Exists()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = GetInMemoryDbContext(dbName);
            var service = new CustomerDataService(context);

            var customer = CreateValidCustomer(3, "Luis");
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var exists = await service.Exists(3);

            Assert.True(exists);
        }

        [Fact]
        public async Task Update_Should_Modify_Existing_Customer()
        {
            var dbName = Guid.NewGuid().ToString();

            var context1 = GetInMemoryDbContext(dbName);
            var service1 = new CustomerDataService(context1);

            var customer = CreateValidCustomer(4, "Maria");
            await service1.Add(customer);

            var context2 = GetInMemoryDbContext(dbName);
            var service2 = new CustomerDataService(context2);

            var updatedCustomer = CreateValidCustomer(4, "Maria Gomez");
            await service2.Update(updatedCustomer);

            var result = await context2.Customers.FindAsync(4);

            Assert.NotNull(result);
            Assert.Equal("Maria Gomez", result!.Name);
        }

        [Fact]
        public async Task Remove_Should_Delete_Customer()
        {
            var dbName = Guid.NewGuid().ToString();
            var context1 = GetInMemoryDbContext(dbName);
            var service1 = new CustomerDataService(context1);

            var customer = CreateValidCustomer(5, "Carlos");
            await service1.Add(customer);

            var context2 = GetInMemoryDbContext(dbName);
            var service2 = new CustomerDataService(context2);

            var customerToDelete = await context2.Customers.FindAsync(5);
            await service2.Remove(customerToDelete!);
            var deleted = await context2.Customers.FindAsync(5);

            Assert.Null(deleted);
        }
    }
}
