using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;
public class ExpensesUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesUpdateOnlyRepository> _repository;
    public ExpensesUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesUpdateOnlyRepository>();
    }
    public ExpensesUpdateOnlyRepositoryBuilder GetById(Expense? expense)
    {
        if (expense != null)
            _repository.Setup(repository => repository.GetById(expense.Id)).ReturnsAsync(expense);
        // Forma de Leitura, Se o usecase (no caso nosso teste) passar corretamente um usuário para o delete, o mock vai retornar true.
        return this;
    }
    public IExpensesUpdateOnlyRepository Build() => _repository.Object;
}
