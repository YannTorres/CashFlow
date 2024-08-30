﻿using CashFlow.Application.UseCases.Users.Register;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    private RegisterUserUseCase CreateUseCase()
    {
        return new RegisterUserUseCase();
    }
}
