using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.UpdateProfile;
public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
