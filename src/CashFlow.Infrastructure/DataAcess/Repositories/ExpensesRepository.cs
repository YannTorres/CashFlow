using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;
internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long Id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(Id));
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long Id) // Função usada para o endpoint de PUT.
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id.Equals(Id)); // Não pode ter o AsNoTracking pois logo depois que pegamos os dados a gente faz a atualização deles.
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id.Equals(id));

        if (result == null) 
            return false;

        _dbContext.Expenses.Remove(result);

        return true;
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        /* var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

         return await _dbContext.Expenses
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync(); */

        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(e => e.Date.Month == date.Month && e.Date.Year == date.Year)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }
}
