using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile;
public class GetProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/User";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;

    public GetProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) 
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _userName = webApplicationFactory.User_Team_Member.GetName();
        _userEmail = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_userName); // Passar a propriedade como camelCase
        response.RootElement.GetProperty("email").GetString().Should().NotBeNullOrEmpty(_userEmail); // Passar a propriedade como camelCase
    }
}
