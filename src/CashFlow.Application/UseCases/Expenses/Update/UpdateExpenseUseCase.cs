using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public UpdateExpenseUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IExpensesUpdateOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _unityOfWork = unitOfWork;
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetById(id);

        if (expense == null || expense.UserId != loggedUser.Id)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        expense.Tags.Clear();

        _mapper.Map(request, expense); //Aqui estamos usando um Map diferente pois nesse caso ele não irá criar uma nova instancia de Expense e sim usar a que a gente passou pra ele.

        _repository.Update(expense);

        await _unityOfWork.Commit();
    }
    private void Validate(RequestExpenseJson request) 
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();


            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
