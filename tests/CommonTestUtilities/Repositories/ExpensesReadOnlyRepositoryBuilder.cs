﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using DocumentFormat.OpenXml.Spreadsheet;
using Moq;

namespace CommonTestUtilities.Repositories;
public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository;
    public ExpensesReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesReadOnlyRepository>();
    }

    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);
        // Forma de Leitura, Se o usecase (no caso nosso teste) passar corretamente um usuário para o GetAll, o mock vai retornar uma lista de Despesas (que também estamos passando).
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense != null)
            _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);
        // Forma de Leitura, Se o usecase (no caso nosso teste) passar corretamente um usuário para o GetAll, o mock vai retornar uma lista de Despesas (que também estamos passando).
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder FilterByMounth(User user, List<Expense> expenses)
    {
        _repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses); // It>IsAny() -> pode ser qualquer dateonly.

        return this;
    }

    public IExpensesReadOnlyRepository Build() => _repository.Object;
}
