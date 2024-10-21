using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
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

    public IExpensesReadOnlyRepository Build() => _repository.Object;
}
