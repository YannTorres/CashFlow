using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(r => r.Password, faker => faker.Internet.Password())
            .RuleFor(r => r.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
