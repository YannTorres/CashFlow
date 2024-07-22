﻿using CashFlow.Communication.Response;

namespace CashFlow.Application.UseCases.Expenses.GetById;
public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
