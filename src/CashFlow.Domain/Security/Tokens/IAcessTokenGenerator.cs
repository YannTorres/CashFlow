using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens;
public interface IAcessTokenGenerator
{
    public string Generate(User user);
}
