namespace CashFlow.Application.UseCases.Expenses.Reports.PDF;
public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
