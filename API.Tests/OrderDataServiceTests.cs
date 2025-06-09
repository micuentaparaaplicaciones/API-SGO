using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class OrderDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "OrderTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Order GetSampleOrder(int id = 1, string status = "Pending")
    {
        return new Order
        {
            Id = id,
            Status = status,
            RegistrationDate = DateTime.UtcNow,
            DeliveryAddress = "123 Main St",
            DeliveryDate = DateTime.UtcNow.AddDays(2),
            CustomerId = "CUST1",
            CustomerName = "Customer Name",
            Subtotal = 100.0m,
            Discount = 10.0m,
            Tax = 5.0m,
            Total = 95.0m,
            PaymentMethod = "Credit Card",
            PaymentStatus = "Paid",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsOrder()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDataService(context);
        var order = GetSampleOrder();

        await service.Add(order);

        Assert.Single(context.Orders);
        Assert.Equal("Pending", context.Orders.First().Status);
    }

    [Fact]
    public async Task GetAll_ReturnsAllOrders()
    {
        var context = GetInMemoryDbContext();
        context.Orders.Add(GetSampleOrder(1, "A"));
        context.Orders.Add(GetSampleOrder(2, "B"));
        context.SaveChanges();

        var service = new OrderDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectOrder()
    {
        var context = GetInMemoryDbContext();
        context.Orders.Add(GetSampleOrder(1, "A"));
        context.Orders.Add(GetSampleOrder(2, "B"));
        context.SaveChanges();

        var service = new OrderDataService(context);

        var order = await service.GetByKey(2);

        Assert.NotNull(order);
        Assert.Equal("B", order.Status);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfOrderExists()
    {
        var context = GetInMemoryDbContext();
        context.Orders.Add(GetSampleOrder(1));
        context.SaveChanges();

        var service = new OrderDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfOrderDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesOrder()
    {
        var context = GetInMemoryDbContext();
        var order = GetSampleOrder(1, "A");
        context.Orders.Add(order);
        context.SaveChanges();

        var service = new OrderDataService(context);

        order.Status = "Updated Status";
        order.Total = 200.0m;
        await service.Update(order);

        var updated = context.Orders.First(o => o.Id == 1);
        Assert.Equal("Updated Status", updated.Status);
        Assert.Equal(200.0m, updated.Total);
    }

    [Fact]
    public async Task Update_ThrowsIfOrderDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDataService(context);
        var order = GetSampleOrder(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(order));
    }

    [Fact]
    public async Task Remove_RemovesOrder()
    {
        var context = GetInMemoryDbContext();
        var order = GetSampleOrder(1, "A");
        context.Orders.Add(order);
        context.SaveChanges();

        var service = new OrderDataService(context);

        await service.Remove(order);

        Assert.Empty(context.Orders);
    }

    [Fact]
    public async Task Remove_ThrowsIfOrderDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDataService(context);
        var order = GetSampleOrder(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(order));
    }

    [Fact]
    public async Task AddMultiple_AddsAllOrders()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDataService(context);
        var orders = new List<Order>
        {
            GetSampleOrder(1, "A"),
            GetSampleOrder(2, "B")
        };

        await service.AddMultiple(orders);

        Assert.Equal(2, context.Orders.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllOrders()
    {
        var context = GetInMemoryDbContext();
        var orders = new List<Order>
        {
            GetSampleOrder(1, "A"),
            GetSampleOrder(2, "B")
        };
        context.Orders.AddRange(orders);
        context.SaveChanges();

        var service = new OrderDataService(context);

        orders[0].Status = "Updated A";
        orders[1].Status = "Updated B";
        await service.UpdateMultiple(orders);

        Assert.Equal("Updated A", context.Orders.First(o => o.Id == 1).Status);
        Assert.Equal("Updated B", context.Orders.First(o => o.Id == 2).Status);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllOrders()
    {
        var context = GetInMemoryDbContext();
        var orders = new List<Order>
        {
            GetSampleOrder(1, "A"),
            GetSampleOrder(2, "B")
        };
        context.Orders.AddRange(orders);
        context.SaveChanges();

        var service = new OrderDataService(context);

        await service.RemoveMultiple(orders);

        Assert.Empty(context.Orders);
    }
}