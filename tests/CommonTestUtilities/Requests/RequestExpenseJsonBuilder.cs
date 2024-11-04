using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        var faker = new Faker();

        return new RequestExpenseJson
        {
            Title = faker.Commerce.ProductName(),
            Date = faker.Date.Past(),
            PaymentType = faker.PickRandom<PaymentType>(),
            Description = faker.Commerce.ProductDescription(),
            Amount = faker.Random.Decimal(min: 1, max: 10000)
        };

        /* new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 10000));
        */
    }
}
