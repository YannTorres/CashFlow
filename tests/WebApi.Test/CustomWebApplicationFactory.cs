using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
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
    public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!; // Forma de dizer que a propriedade não vai ser nula.
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!; // Forma de dizer que a propriedade não vai ser nula.
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
        var userTeamMember = AddUsersTeamMember(dbContext, passwordEncripter, acessTokenGenerator);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1, tagId: 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

        var userAdmin = AddUsersAdmin(dbContext, passwordEncripter, acessTokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2, tagId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        dbContext.SaveChanges();
    }

    private User AddUsersTeamMember(CashFlowDbContext dbContext, 
        IPasswordEncripter passwordEncripter, 
        IAcessTokenGenerator acessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;
        var password = user.Password;

        user.Password = passwordEncripter.Encript(user.Password);
        dbContext.Users.Add(user);

        var token = acessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUsersAdmin(CashFlowDbContext dbContext, 
        IPasswordEncripter passwordEncripter, 
        IAcessTokenGenerator acessTokenGenerator)
    {
        var user = UserBuilder.Build(role: Roles.ADMIN);
        user.Id = 2;
        var password = user.Password;

        user.Password = passwordEncripter.Encript(user.Password);
        dbContext.Users.Add(user);

        var token = acessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId, long tagId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expenseId;
        }

        dbContext.Expenses.Add(expense);

        return expense;
    }
}
