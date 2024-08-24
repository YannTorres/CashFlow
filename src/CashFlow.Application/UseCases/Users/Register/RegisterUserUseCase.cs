using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPassworkEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _repositoryReadOnly;
    private readonly IUserWriteOnlyRepository _repositoryWriteOnly;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserUseCase(
        IMapper mapper,
        IPassworkEncripter passwordEncripter,
        IUserReadOnlyRepository repositoryReadOnly,
        IUserWriteOnlyRepository repositoryWriteOnly,
        IUnitOfWork unitOfWork
        )
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _repositoryReadOnly = repositoryReadOnly;
        _repositoryWriteOnly = repositoryWriteOnly;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisteredUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encript(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _repositoryWriteOnly.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
        };
    }
    private async Task Validate(RequestRegisteredUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var existEmail = await _repositoryReadOnly.ExistUserWithEmail(request.Email);

        if (existEmail)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

    }
}
