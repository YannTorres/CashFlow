using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using CashFlow.Application.UseCases.Users.UpdateProfile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CashFlow.Application.UseCases.Users.ChangePassword;
using CommonTestUtilities.Cryptography;

namespace UseCases.Tests.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && 
            ex.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFERENT_CURRENT_PASSWORD));
    }

    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, updateRepository, unitOfWork, passwordEncripter);
    }
}
