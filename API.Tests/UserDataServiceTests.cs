using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class UserDataServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "UserTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private User GetSampleUser(int id = 1, string email = "test@email.com")
    {
        return new User
        {
            Id = id,
            Name = "Test User",
            Email = email,
            Phone = "1234567890",
            Address = "Test Address",
            Password = "password",
            Role = "Admin",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
    }

    [Fact]
    public async Task Add_AddsUser()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);
        var user = GetSampleUser();

        await service.Add(user);

        Assert.Single(context.Users);
        Assert.Equal("Test User", context.Users.First().Name);
    }

    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(GetSampleUser(1, "a@email.com"));
        context.Users.Add(GetSampleUser(2, "b@email.com"));
        context.SaveChanges();

        var service = new UserDataService(context);

        var result = await service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByKey_ReturnsCorrectUser()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(GetSampleUser(1, "a@email.com"));
        context.Users.Add(GetSampleUser(2, "b@email.com"));
        context.SaveChanges();

        var service = new UserDataService(context);

        var user = await service.GetByKey(2);

        Assert.NotNull(user);
        Assert.Equal("b@email.com", user.Email);
    }

    [Fact]
    public async Task Exists_ReturnsTrueIfUserExists()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(GetSampleUser(1));
        context.SaveChanges();

        var service = new UserDataService(context);

        var exists = await service.Exists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);

        var exists = await service.Exists(99);

        Assert.False(exists);
    }

    [Fact]
    public async Task GetByEmail_ReturnsCorrectUser()
    {
        var context = GetInMemoryDbContext();
        context.Users.Add(GetSampleUser(1, "a@email.com"));
        context.Users.Add(GetSampleUser(2, "b@email.com"));
        context.SaveChanges();

        var service = new UserDataService(context);

        var user = await service.GetByEmail("b@email.com");

        Assert.NotNull(user);
        Assert.Equal(2, user.Id);
    }

    [Fact]
    public async Task GetByEmail_ReturnsNullIfNotExists()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);

        var user = await service.GetByEmail("noexiste@email.com");

        Assert.Null(user);
    }

    [Fact]
    public async Task Update_UpdatesUser()
    {
        var context = GetInMemoryDbContext();
        var user = GetSampleUser(1, "a@email.com");
        context.Users.Add(user);
        context.SaveChanges();

        var service = new UserDataService(context);

        user.Name = "Updated Name";
        await service.Update(user);

        var updated = context.Users.First(u => u.Id == 1);
        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task Update_ThrowsIfUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);
        var user = GetSampleUser(99, "noexiste@email.com");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Update(user));
    }

    [Fact]
    public async Task Remove_RemovesUser()
    {
        var context = GetInMemoryDbContext();
        var user = GetSampleUser(1, "a@email.com");
        context.Users.Add(user);
        context.SaveChanges();

        var service = new UserDataService(context);

        await service.Remove(user);

        Assert.Empty(context.Users);
    }

    [Fact]
    public async Task Remove_ThrowsIfUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);
        var user = GetSampleUser(99, "noexiste@email.com");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remove(user));
    }

    [Fact]
    public async Task AddMultiple_AddsAllUsers()
    {
        var context = GetInMemoryDbContext();
        var service = new UserDataService(context);
        var users = new List<User>
        {
            GetSampleUser(1, "a@email.com"),
            GetSampleUser(2, "b@email.com")
        };

        await service.AddMultiple(users);

        Assert.Equal(2, context.Users.Count());
    }

    [Fact]
    public async Task UpdateMultiple_UpdatesAllUsers()
    {
        var context = GetInMemoryDbContext();
        var users = new List<User>
        {
            GetSampleUser(1, "a@email.com"),
            GetSampleUser(2, "b@email.com")
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        var service = new UserDataService(context);

        users[0].Name = "Updated A";
        users[1].Name = "Updated B";
        await service.UpdateMultiple(users);

        Assert.Equal("Updated A", context.Users.First(u => u.Id == 1).Name);
        Assert.Equal("Updated B", context.Users.First(u => u.Id == 2).Name);
    }

    [Fact]
    public async Task RemoveMultiple_RemovesAllUsers()
    {
        var context = GetInMemoryDbContext();
        var users = new List<User>
        {
            GetSampleUser(1, "a@email.com"),
            GetSampleUser(2, "b@email.com")
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        var service = new UserDataService(context);

        await service.RemoveMultiple(users);

        Assert.Empty(context.Users);
    }
}