using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess;
internal class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { } // Nesse cenário estamos passando informações de conexão para o construtor da classe base

    public DbSet<Expense> Expenses { get; set; }
}