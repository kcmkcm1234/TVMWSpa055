﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IOtherExpenseBusiness
    {
        List<OtherExpense> GetAllOtherExpenses();
        OtherExpense GetExpenseDetailsByID(Guid ID);
        List<ChartOfAccounts> GetAllAccountTypes(string accountType);
        List<Companies> GetAllCompanies();
        List<PaymentModes> GetAllPaymentModes();
        List<Bank> GetAllBankes();
        List<Employee> GetAllEmployees();
        List<EmployeeType> GetAllEmployeeTypes();
        List<Employee> GetAllEmployeesByType(string Type);
        List<Employee> GetCompanybyEmployee(Guid ID);
        OtherExpense InsertOtherExpense(OtherExpense otherExpense);
        OtherExpense UpdateOtherExpense(OtherExpense otherExpense);
        object DeleteOtherExpense(Guid ID, string UserName);
        OtherExpSummary GetOtherExpSummary(int month, int year, string Company);
        List<OtherExpense> GetExpenseTypeDetails(OtherExpense expObj);
    }
}