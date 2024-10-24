﻿using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;
public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;
    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}
