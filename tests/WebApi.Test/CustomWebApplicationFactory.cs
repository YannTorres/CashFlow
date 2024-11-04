using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAcess;
using CommonTestUtilities.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense { get; private set; } = default!; // Forma de dizer que a propriedade não vai ser nula.
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
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
                var acessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAcessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, acessTokenGenerator); // Persistir as alterações no banco
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGenerator acessTokenGenerator) 
    {
        var user = AddUsersTeamMember(dbContext, passwordEncripter, acessTokenGenerator);
        AddExpenses(dbContext, user);

        dbContext.SaveChanges();
    }

    private User AddUsersTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGenerator acessTokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;

        user.Password = passwordEncripter.Encript(user.Password);
        dbContext.Users.Add(user);

        var token = acessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(expense);

        Expense = new ExpenseIdentityManager(expense);
    }
}
