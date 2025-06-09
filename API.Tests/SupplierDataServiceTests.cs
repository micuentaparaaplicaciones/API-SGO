using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class SupplierDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SupplierTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Supplier GetSampleSupplier(int id = 1, string name = "Test Supplier")
    {
        return new Supplier
        {
            Id = id,
            Name = name,
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsSupplier()
    {
        var context = GetInMemoryDbContext();
        var service = new SupplierDataService(context);
        var supplier = GetSampleSupplier();

        await service.Add(supplier);

        Assert.Single(context.Suppliers);
        Assert.Equal("Test Supplier", context.Suppliers.First().Name);
    }

    [Fact]
    public async Task GetAll_ReturnsAllSuppliers()
    {
        var context = GetInMemoryDbContext();
        context.Suppliers.Add(GetSampleSupplier(1, "A"));
        context.Suppliers.Add(GetSampleSupplier(2, "B"));
        context.SaveChanges();

        var service = new SupplierDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectSupplier()
    {
        var context = GetInMemoryDbContext();
        context.Suppliers.Add(GetSampleSupplier(1, "A"));
        context.Suppliers.Add(GetSampleSupplier(2, "B"));
        context.SaveChanges();

        var service = new SupplierDataService(context);

        var supplier = await service.GetByKey(2);

        Assert.NotNull(supplier);
        Assert.Equal("B", supplier.Name);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfSupplierExists()
    {
        var context = GetInMemoryDbContext();
        context.Suppliers.Add(GetSampleSupplier(1));
        context.SaveChanges();

        var service = new SupplierDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfSupplierDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new SupplierDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesSupplier()
    {
        var context = GetInMemoryDbContext();
        var supplier = GetSampleSupplier(1, "A");
        context.Suppliers.Add(supplier);
        context.SaveChanges();

        var service = new SupplierDataService(context);

        supplier.Name = "Updated Name";
        await service.Update(supplier);

        var updated = context.Suppliers.First(s => s.Id == 1);
        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task Update_ThrowsIfSupplierDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new SupplierDataService(context);
        var supplier = GetSampleSupplier(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(supplier));
    }

    [Fact]
    public async Task Remove_RemovesSupplier()
    {
        var context = GetInMemoryDbContext();
        var supplier = GetSampleSupplier(1, "A");
        context.Suppliers.Add(supplier);
        context.SaveChanges();

        var service = new SupplierDataService(context);

        await service.Remove(supplier);

        Assert.Empty(context.Suppliers);
    }

    [Fact]
    public async Task Remove_ThrowsIfSupplierDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new SupplierDataService(context);
        var supplier = GetSampleSupplier(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(supplier));
    }

    [Fact]
    public async Task AddMultiple_AddsAllSuppliers()
    {
        var context = GetInMemoryDbContext();
        var service = new SupplierDataService(context);
        var suppliers = new List<Supplier>
        {
            GetSampleSupplier(1, "A"),
            GetSampleSupplier(2, "B")
        };

        await service.AddMultiple(suppliers);

        Assert.Equal(2, context.Suppliers.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllSuppliers()
    {
        var context = GetInMemoryDbContext();
        var suppliers = new List<Supplier>
        {
            GetSampleSupplier(1, "A"),
            GetSampleSupplier(2, "B")
        };
        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        var service = new SupplierDataService(context);

        suppliers[0].Name = "Updated A";
        suppliers[1].Name = "Updated B";
        await service.UpdateMultiple(suppliers);

        Assert.Equal("Updated A", context.Suppliers.First(s => s.Id == 1).Name);
        Assert.Equal("Updated B", context.Suppliers.First(s => s.Id == 2).Name);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllSuppliers()
    {
        var context = GetInMemoryDbContext();
        var suppliers = new List<Supplier>
        {
            GetSampleSupplier(1, "A"),
            GetSampleSupplier(2, "B")
        };
        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        var service = new SupplierDataService(context);

        await service.RemoveMultiple(suppliers);

        Assert.Empty(context.Suppliers);
    }
}