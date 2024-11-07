using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today)); // conversão de um date time do dia atual para dateonly

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Sucess_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, new List<Expense>());

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today)); // conversão de um date time do dia atual para dateonly

        result.Should().BeEmpty();
    }

    private GenerateExpensesReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMounth(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportExcelUseCase(repository, loggedUser);
    }
}
