using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser, 
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute()
    {
        var user = await _loggedUser.Get();

        await _writeOnlyRepository.Delete(user);

        await _unitOfWork.Commit();
    }
}
