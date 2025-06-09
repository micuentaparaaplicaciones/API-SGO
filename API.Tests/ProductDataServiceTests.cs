using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class ProductDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Product GetSampleProduct(int id = 1, string name = "Test Product")
    {
        return new Product
        {
            Id = id,
            Name = name,
            Detail = "Detail",
            Price = 100.0m,
            AvailableQuantity = 10,
            Supplier = "Supplier1",
            Category = "Category1",
            CreatedBy = "System",
            ModifiedBy = "System",
            Image = new byte[] { 1, 2, 3, 4 }
        };
    }

    [Fact]
    public async Task Add_AddsProductWithImage()
    {
        var context = GetInMemoryDbContext();
        var service = new ProductDataService(context);
        var product = GetSampleProduct();

        await service.Add(product);

        var saved = context.Products.First();
        Assert.Equal("Test Product", saved.Name);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, saved.Image);
    }

    [Fact]
    public async Task GetAll_ReturnsAllProductsWithImages()
    {
        var context = GetInMemoryDbContext();
        context.Products.Add(GetSampleProduct(1, "A"));
        context.Products.Add(GetSampleProduct(2, "B"));
        context.SaveChanges();

        var service = new ProductDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.NotNull(p.Image));
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectProductWithImage()
    {
        var context = GetInMemoryDbContext();
        context.Products.Add(GetSampleProduct(1, "A"));
        context.Products.Add(GetSampleProduct(2, "B"));
        context.SaveChanges();

        var service = new ProductDataService(context);

        var product = await service.GetByKey(2);

        Assert.NotNull(product);
        Assert.Equal("B", product.Name);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, product.Image);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfProductExists()
    {
        var context = GetInMemoryDbContext();
        context.Products.Add(GetSampleProduct(1));
        context.SaveChanges();

        var service = new ProductDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfProductDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new ProductDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesProductAndImage()
    {
        var context = GetInMemoryDbContext();
        var product = GetSampleProduct(1, "A");
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductDataService(context);

        product.Name = "Updated Name";
        product.Image = new byte[] { 9, 8, 7 };
        await service.Update(product);

        var updated = context.Products.First(p => p.Id == 1);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal(new byte[] { 9, 8, 7 }, updated.Image);
    }

    [Fact]
    public async Task Update_ThrowsIfProductDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new ProductDataService(context);
        var product = GetSampleProduct(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(product));
    }

    [Fact]
    public async Task Remove_RemovesProduct()
    {
        var context = GetInMemoryDbContext();
        var product = GetSampleProduct(1, "A");
        context.Products.Add(product);
        context.SaveChanges();

        var service = new ProductDataService(context);

        await service.Remove(product);

        Assert.Empty(context.Products);
    }

    [Fact]
    public async Task Remove_ThrowsIfProductDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new ProductDataService(context);
        var product = GetSampleProduct(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(product));
    }

    [Fact]
    public async Task AddMultiple_AddsAllProductsWithImages()
    {
        var context = GetInMemoryDbContext();
        var service = new ProductDataService(context);
        var products = new List<Product>
        {
            GetSampleProduct(1, "A"),
            GetSampleProduct(2, "B")
        };

        await service.AddMultiple(products);

        Assert.Equal(2, context.Products.Count());
        Assert.All(context.Products, p => Assert.NotNull(p.Image));
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllProductsAndImages()
    {
        var context = GetInMemoryDbContext();
        var products = new List<Product>
        {
            GetSampleProduct(1, "A"),
            GetSampleProduct(2, "B")
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        var service = new ProductDataService(context);

        products[0].Name = "Updated A";
        products[0].Image = new byte[] { 5, 5, 5 };
        products[1].Name = "Updated B";
        products[1].Image = new byte[] { 6, 6, 6 };
        await service.UpdateMultiple(products);

        Assert.Equal("Updated A", context.Products.First(p => p.Id == 1).Name);
        Assert.Equal(new byte[] { 5, 5, 5 }, context.Products.First(p => p.Id == 1).Image);
        Assert.Equal("Updated B", context.Products.First(p => p.Id == 2).Name);
        Assert.Equal(new byte[] { 6, 6, 6 }, context.Products.First(p => p.Id == 2).Image);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllProducts()
    {
        var context = GetInMemoryDbContext();
        var products = new List<Product>
        {
            GetSampleProduct(1, "A"),
            GetSampleProduct(2, "B")
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        var service = new ProductDataService(context);

        await service.RemoveMultiple(products);

        Assert.Empty(context.Products);
    }
}