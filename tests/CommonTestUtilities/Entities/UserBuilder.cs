using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build(); // Mock valor criptografado

        var user = new Faker<User>()
            .RuleFor(u => u.Id, faker => faker.Random.Int(min: 0, max: 1000))
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(firstName: user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encript(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid()) // _ serve para desconsiderar o próximo parametro (Faker)
            .RuleFor(u => u.Role, _ => role);
        
        return user;
    }

}
