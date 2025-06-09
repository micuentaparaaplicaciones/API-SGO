using API.Controllers;
using API.DataServices;
using API.DbContexts;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AuthControllerTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthTestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private IConfiguration GetFakeConfiguration()
    {
        var dict = new Dictionary<string, string>
        {
            { "Jwt:Key", "clave-secreta-muy-segura-para-tu-app" },
            { "Jwt:Issuer", "API SGO" },
            { "Jwt:Audience", "API SGO" }
        };
        return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
    }

    [Fact]
    public async Task Register_CreatesUser_And_HashesPassword()
    {
        var context = GetInMemoryDbContext();
        var userService = new UserDataService(context);
        var config = GetFakeConfiguration();
        var controller = new AuthController(userService, config);

        var dto = new RegisterDto
        {
            Name = "Test",
            Email = "test@email.com",
            Phone = "123456789",
            Address = "Test Address",
            Password = "Password123!",
            Role = "User",
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        var result = await controller.Register(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var user = await userService.GetByEmail(dto.Email);
        Assert.NotNull(user);
        Assert.NotEqual("Password123!", user.Password); // Debe estar hasheada
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_IfEmailExists()
    {
        var context = GetInMemoryDbContext();
        var userService = new UserDataService(context);
        var config = GetFakeConfiguration();
        var controller = new AuthController(userService, config);

        // Registrar usuario
        var dto = new RegisterDto
        {
            Name = "Test",
            Email = "test@email.com",
            Phone = "123456789",
            Address = "Test Address",
            Password = "Password123!",
            Role = "User",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
        await controller.Register(dto);

        // Intentar registrar el mismo email
        var result = await controller.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        var context = GetInMemoryDbContext();
        var userService = new UserDataService(context);
        var config = GetFakeConfiguration();
        var controller = new AuthController(userService, config);

        // Registrar usuario
        var dto = new RegisterDto
        {
            Name = "Test",
            Email = "test@email.com",
            Phone = "123456789",
            Address = "Test Address",
            Password = "Password123!",
            Role = "User",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
        await controller.Register(dto);

        // Login válido
        var loginDto = new LoginDto
        {
            Email = "test@email.com",
            Password = "Password123!"
        };

        var result = await controller.Login(loginDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("token", okResult.Value.ToString());
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        var context = GetInMemoryDbContext();
        var userService = new UserDataService(context);
        var config = GetFakeConfiguration();
        var controller = new AuthController(userService, config);

        // Registrar usuario
        var dto = new RegisterDto
        {
            Name = "Test",
            Email = "test@email.com",
            Phone = "123456789",
            Address = "Test Address",
            Password = "Password123!",
            Role = "User",
            CreatedBy = "System",
            ModifiedBy = "System"
        };
        await controller.Register(dto);

        // Login con contraseña incorrecta
        var loginDto = new LoginDto
        {
            Email = "test@email.com",
            Password = "WrongPassword"
        };

        var result = await controller.Login(loginDto);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        var context = GetInMemoryDbContext();
        var userService = new UserDataService(context);
        var config = GetFakeConfiguration();
        var controller = new AuthController(userService, config);

        var loginDto = new LoginDto
        {
            Email = "noexiste@email.com",
            Password = "Password123!"
        };

        var result = await controller.Login(loginDto);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}