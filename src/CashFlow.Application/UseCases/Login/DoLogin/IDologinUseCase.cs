using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;

namespace CashFlow.Application.UseCases.Login.DoLogin;
public interface IDologinUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
