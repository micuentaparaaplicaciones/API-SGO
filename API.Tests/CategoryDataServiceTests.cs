using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class CategoryDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CategoryTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Category GetSampleCategory(int id = 1, string name = "Test Category")
    {
        return new Category
        {
            Id = id,
            Name = name,
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsCategory()
    {
        var context = GetInMemoryDbContext();
        var service = new CategoryDataService(context);
        var category = GetSampleCategory();

        await service.Add(category);

        Assert.Single(context.Categories);
        Assert.Equal("Test Category", context.Categories.First().Name);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCategories()
    {
        var context = GetInMemoryDbContext();
        context.Categories.Add(GetSampleCategory(1, "A"));
        context.Categories.Add(GetSampleCategory(2, "B"));
        context.SaveChanges();

        var service = new CategoryDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectCategory()
    {
        var context = GetInMemoryDbContext();
        context.Categories.Add(GetSampleCategory(1, "A"));
        context.Categories.Add(GetSampleCategory(2, "B"));
        context.SaveChanges();

        var service = new CategoryDataService(context);

        var category = await service.GetByKey(2);

        Assert.NotNull(category);
        Assert.Equal("B", category.Name);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfCategoryExists()
    {
        var context = GetInMemoryDbContext();
        context.Categories.Add(GetSampleCategory(1));
        context.SaveChanges();

        var service = new CategoryDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CategoryDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task Update_UpdatesCategory()
    {
        var context = GetInMemoryDbContext();
        var category = GetSampleCategory(1, "A");
        context.Categories.Add(category);
        context.SaveChanges();

        var service = new CategoryDataService(context);

        category.Name = "Updated Name";
        await service.Update(category);

        var updated = context.Categories.First(c => c.Id == 1);
        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task Update_ThrowsIfCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CategoryDataService(context);
        var category = GetSampleCategory(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(category));
    }

    [Fact]
    public async Task Remove_RemovesCategory()
    {
        var context = GetInMemoryDbContext();
        var category = GetSampleCategory(1, "A");
        context.Categories.Add(category);
        context.SaveChanges();

        var service = new CategoryDataService(context);

        await service.Remove(category);

        Assert.Empty(context.Categories);
    }

    [Fact]
    public async Task Remove_ThrowsIfCategoryDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new CategoryDataService(context);
        var category = GetSampleCategory(99, "NoExiste");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(category));
    }

    [Fact]
    public async Task AddMultiple_AddsAllCategories()
    {
        var context = GetInMemoryDbContext();
        var service = new CategoryDataService(context);
        var categories = new List<Category>
        {
            GetSampleCategory(1, "A"),
            GetSampleCategory(2, "B")
        };

        await service.AddMultiple(categories);

        Assert.Equal(2, context.Categories.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllCategories()
    {
        var context = GetInMemoryDbContext();
        var categories = new List<Category>
        {
            GetSampleCategory(1, "A"),
            GetSampleCategory(2, "B")
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var service = new CategoryDataService(context);

        categories[0].Name = "Updated A";
        categories[1].Name = "Updated B";
        await service.UpdateMultiple(categories);

        Assert.Equal("Updated A", context.Categories.First(c => c.Id == 1).Name);
        Assert.Equal("Updated B", context.Categories.First(c => c.Id == 2).Name);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllCategories()
    {
        var context = GetInMemoryDbContext();
        var categories = new List<Category>
        {
            GetSampleCategory(1, "A"),
            GetSampleCategory(2, "B")
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var service = new CategoryDataService(context);

        await service.RemoveMultiple(categories);

        Assert.Empty(context.Categories);
    }
}