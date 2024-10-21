using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;
    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistUserWithEmail(string email)
    {
        _repository.Setup(userReadOnly => userReadOnly.ExistUserWithEmail(email)).ReturnsAsync(true);
    }
    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this; // Neste caso estamos devolvendo a própria instancia da classe, que serve para chamar o Build após essa função.
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
