using CashFlow.Infrastructure.DataAcess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace CashFlow.Infrastructure.Migrations;
public static class DataBaseMigration
{
    public static async Task MigrateDataBase(IServiceProvider serviceProvider)
    {
        var dbcontext = serviceProvider.GetRequiredService<CashFlowDbContext>();

        await dbcontext.Database.MigrateAsync();
    }
}
