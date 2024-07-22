using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public interface IGetAllExpenseUseCase
{
    Task<ResponseExpensesJson> Execute();
}
