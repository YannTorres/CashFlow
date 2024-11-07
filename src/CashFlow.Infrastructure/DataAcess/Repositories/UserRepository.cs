using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbcontext;
    public UserRepository(CashFlowDbContext dbContext)
    {
        _dbcontext = dbContext;    
    }

    public async Task Add(User user)
    {
        await _dbcontext.Users.AddAsync(user);
    }

    public async Task<bool> ExistUserWithEmail(string email)
    {
        return await _dbcontext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> GetById(long id)
    {
        return await _dbcontext.Users.FirstAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user)
    {
        _dbcontext.Users.Update(user);
    }
}
