using CashFlow.Communication.Response;

namespace CashFlow.Application.UseCases.Users.GetProfile;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
