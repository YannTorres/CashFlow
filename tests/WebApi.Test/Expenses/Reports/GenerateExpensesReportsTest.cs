using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Reports;
public class GenerateExpensesReportsTest : CashFlowClassFixture
{
    private const string METHOD = "api/Report";
    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;
    public GenerateExpensesReportsTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Sucess_Pdf()
    {
        var month = _expenseDate.Month.ToString("D2");
        var year = _expenseDate.Year; 

        var result = await DoGet($"{METHOD}/pdf?month={year}-{month}", _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Sucess_Excel()
    {
        var month = _expenseDate.Month.ToString("D2");
        var year = _expenseDate.Year;

        var result = await DoGet($"{METHOD}/excel?month={year}-{month}", _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
