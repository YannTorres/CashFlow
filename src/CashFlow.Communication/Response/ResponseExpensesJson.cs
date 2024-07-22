using CashFlow.Communication.Response;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public class ResponseExpensesJson
{
    public List<ResponseShortExpenseJson> Expenses { get; set; } = [];
}
