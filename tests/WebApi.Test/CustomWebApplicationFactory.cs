using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAcess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;
    private string _token;
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
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAcessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter); // Persistir as alterações no banco

                _token = tokenGenerator.Generate(_user);
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
    public string GetToken()
    {
        return _token;
    }


    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncrripter) 
    {
        AddUsers(dbContext, passwordEncrripter);
        AddExpenses(dbContext, _user);

        dbContext.SaveChanges();
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();

        _password = _user.Password;
        _user.Password = passwordEncripter.Encript(_user.Password);

        dbContext.Users.Add(_user); 
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(expense);
    }
}
