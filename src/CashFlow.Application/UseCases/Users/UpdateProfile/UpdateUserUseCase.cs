using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Users.UpdateProfile;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id); // Precisamos recuperar o usuário novamente pois no loggged user estamos buscando usando o AsNoTracking

        user.Name = request.Name;
        user.Email = request.Email;

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _userReadOnlyRepository.ExistUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
