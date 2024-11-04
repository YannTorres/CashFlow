using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;
public class UserIdentityManager
{
    private readonly User _user;
    private readonly string _password;
    private readonly string _token;

    public UserIdentityManager(User user, string password, string token)
    {
        _password = password;
        _token = token;
        _user = user;
    }

    public string GetEmail()
    {
        return _user.Email;
    }
    public string GetName()
    {
        return _user.Name;
    }
    public string GetPassword()
    {
        return _password;
    }
    public string GetToken()
    {
        return _token;
    }
}
