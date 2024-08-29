using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDologinUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _encripter;
    private readonly IAcessTokenGenerator _acessTokenGenerator;
    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter encripter, 
        IAcessTokenGenerator acessTokenGenerator
        )
    {
        _acessTokenGenerator = acessTokenGenerator;
        _repository = repository;
        _encripter = encripter;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetUserByEmail(request.Email);
        if (user == null)
        {
            throw new InvalidLoginException();
        }

        var passwordMatch = _encripter.Verify(request.Password, user.Password);

        if (passwordMatch == false)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _acessTokenGenerator.Generate(user)
        };
    }
}
