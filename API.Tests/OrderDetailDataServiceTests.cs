using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class OrderDetailDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "OrderDetailTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private OrderDetail GetSampleOrderDetail(int orderId = 1, int productId = 1, string status = "Pending")
    {
        return new OrderDetail
        {
            OrderId = orderId,
            ProductId = productId,
            ProductName = $"Product {productId}",
            Status = status,
            Notes = "Some notes",
            ProductRequestedQuantity = 2,
            ProductPrice = 50.0m,
            Subtotal = 100.0m,
            Discount = 10.0m,
            Tax = 5.0m,
            Total = 95.0m,
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsOrderDetail()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDetailDataService(context);
        var detail = GetSampleOrderDetail();

        await service.Add(detail);

        Assert.Single(context.OrderDetails);
        Assert.Equal("Product 1", context.OrderDetails.First().ProductName);
    }

    [Fact]
    public async Task GetAll_ReturnsAllOrderDetails()
    {
        var context = GetInMemoryDbContext();
        context.OrderDetails.Add(GetSampleOrderDetail(1, 1));
        context.OrderDetails.Add(GetSampleOrderDetail(1, 2));
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectOrderDetail()
    {
        var context = GetInMemoryDbContext();
        context.OrderDetails.Add(GetSampleOrderDetail(1, 1));
        context.OrderDetails.Add(GetSampleOrderDetail(2, 2));
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        var detail = await service.GetByKey((2, 2));

        Assert.NotNull(detail);
        Assert.Equal(2, detail.OrderId);
        Assert.Equal(2, detail.ProductId);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfOrderDetailExists()
    {
        var context = GetInMemoryDbContext();
        context.OrderDetails.Add(GetSampleOrderDetail(1, 1));
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        var exists = await service.Exists((1, 1));

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfOrderDetailDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDetailDataService(context);

        var exists = await service.Exists((99, 99));

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesOrderDetail()
    {
        var context = GetInMemoryDbContext();
        var detail = GetSampleOrderDetail(1, 1, "Pending");
        context.OrderDetails.Add(detail);
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        detail.Status = "Completed";
        detail.Notes = "Updated notes";
        await service.Update(detail);

        var updated = context.OrderDetails.First(od => od.OrderId == 1 && od.ProductId == 1);
        Assert.Equal("Completed", updated.Status);
        Assert.Equal("Updated notes", updated.Notes);
    }

    [Fact]
    public async Task Update_ThrowsIfOrderDetailDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDetailDataService(context);
        var detail = GetSampleOrderDetail(99, 99);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(detail));
    }

    [Fact]
    public async Task Remove_RemovesOrderDetail()
    {
        var context = GetInMemoryDbContext();
        var detail = GetSampleOrderDetail(1, 1);
        context.OrderDetails.Add(detail);
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        await service.Remove(detail);

        Assert.Empty(context.OrderDetails);
    }

    [Fact]
    public async Task Remove_ThrowsIfOrderDetailDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDetailDataService(context);
        var detail = GetSampleOrderDetail(99, 99);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(detail));
    }

    [Fact]
    public async Task AddMultiple_AddsAllOrderDetails()
    {
        var context = GetInMemoryDbContext();
        var service = new OrderDetailDataService(context);
        var details = new List<OrderDetail>
        {
            GetSampleOrderDetail(1, 1),
            GetSampleOrderDetail(1, 2)
        };

        await service.AddMultiple(details);

        Assert.Equal(2, context.OrderDetails.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllOrderDetails()
    {
        var context = GetInMemoryDbContext();
        var details = new List<OrderDetail>
        {
            GetSampleOrderDetail(1, 1, "Pending"),
            GetSampleOrderDetail(1, 2, "Pending")
        };
        context.OrderDetails.AddRange(details);
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        details[0].Status = "Updated 1";
        details[1].Status = "Updated 2";
        await service.UpdateMultiple(details);

        Assert.Equal("Updated 1", context.OrderDetails.First(od => od.ProductId == 1).Status);
        Assert.Equal("Updated 2", context.OrderDetails.First(od => od.ProductId == 2).Status);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllOrderDetails()
    {
        var context = GetInMemoryDbContext();
        var details = new List<OrderDetail>
        {
            GetSampleOrderDetail(1, 1),
            GetSampleOrderDetail(1, 2)
        };
        context.OrderDetails.AddRange(details);
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        await service.RemoveMultiple(details);

        Assert.Empty(context.OrderDetails);
    }

    [Fact]
    public async Task GetOrderDetails_ReturnsAllDetailsForOrder()
    {
        var context = GetInMemoryDbContext();
        context.OrderDetails.Add(GetSampleOrderDetail(1, 1));
        context.OrderDetails.Add(GetSampleOrderDetail(1, 2));
        context.OrderDetails.Add(GetSampleOrderDetail(2, 1));
        context.SaveChanges();

        var service = new OrderDetailDataService(context);

        var result = await service.GetOrderDetails(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, od => Assert.Equal(1, od.OrderId));
    }
}