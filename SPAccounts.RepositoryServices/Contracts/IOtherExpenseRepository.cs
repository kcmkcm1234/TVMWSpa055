﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IOtherExpenseRepository
    {
        List<OtherExpense> GetAllOtherExpenses();
        OtherExpense InsertOtherExpense(OtherExpense otherExpense);
        OtherExpense UpdateOtherExpense(OtherExpense otherExpense);
        object DeleteOtherExpense(Guid ID, string UserName);
        OtherExpSummary GetOtherExpSummary(int month, int year, string Company);
        List<OtherExpense> GetExpenseTypeDetails(OtherExpense expObj);
    }
}