﻿using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;
public class ExpenseIdentityManager
{
    private readonly Expense _expense;

    public ExpenseIdentityManager(Expense expense)
    {
        _expense = expense;
    }

    public long GetExpenseId()
    {
        return _expense.Id;
    }
    public DateTime GetDate()
    {
        return _expense.Date;
    }
}
