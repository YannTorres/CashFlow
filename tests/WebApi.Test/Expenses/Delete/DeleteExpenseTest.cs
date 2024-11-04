using CashFlow.Communication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;
public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;
    private readonly long _expenseId;
    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetExpenseId();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoDelete($"{METHOD}/{_expenseId}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet($"{METHOD}/{_expenseId}", _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound); // Essas duas ultimas linhas tem o objetivo de verificar se realmente foi deletado dando um get na despesa deletada.
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var result = await DoDelete($"{METHOD}/1000", _token, culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new System.Globalization.CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}
