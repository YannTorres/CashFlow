using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Users.Register;
public class RegisterUserTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/User";

    private readonly HttpClient _httpClient;
    public RegisterUserTests(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();   
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name); // Passar a propriedade como camelCase
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty(); // Passar a propriedade como camelCase
    }
}
