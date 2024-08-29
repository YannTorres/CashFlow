namespace CashFlow.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email); 
}
