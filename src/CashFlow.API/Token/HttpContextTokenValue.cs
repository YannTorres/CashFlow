using CashFlow.Domain.Security.Tokens;

namespace CashFlow.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization.Substring(7, authorization.Length).Trim();
        // return authorization["Bearer ".Lenght..].Trim();
    }
}
