using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;
public class ExpensesWriteOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesWriteOnlyRepository> _repository;
    public ExpensesWriteOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesWriteOnlyRepository>();
    }
    public ExpensesWriteOnlyRepositoryBuilder Remove(User user, Expense? expense)
    {
        if (expense != null)
            _repository.Setup(repository => repository.Delete(user, expense.Id)).ReturnsAsync(true);
        // Forma de Leitura, Se o usecase (no caso nosso teste) passar corretamente um usuário para o delete, o mock vai retornar true.
        return this;
    }
    public IExpensesWriteOnlyRepository BuildRemove() => _repository.Object;
    public static IExpensesWriteOnlyRepository Build()
    {
        var mock = new Mock<IExpensesWriteOnlyRepository>();

        return mock.Object;
    }
}
