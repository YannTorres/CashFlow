using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAcess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private CashFlow.Domain.Entities.User _user;
    private string _password;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope(); // Criando um scopo como se fosse injeção de dependências
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>(); // Configuramos o db context para aquele scopo da injeção de dependências
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>(); 

                StartDatabase(dbContext, passwordEncripter); // Persistir as alterações no banco
            });
    }

    public string GetEmail()
    {
        return _user.Email;
    }

    public string GetName()
    {
        return _user.Name;
    }

    public string GetPassword()
    {
        return _password;
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncrripter) 
    {
        _user = UserBuilder.Build();

        _password = _user.Password;
        _user.Password = passwordEncrripter.Encript(_user.Password);

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}
