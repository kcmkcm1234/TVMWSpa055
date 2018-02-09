using SPAccounts.DataAccessObject.DTO;
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
        List<OtherExpense> GetReversalReference(string EmpID, string AccountCode, string EmpTypeCode);
        OtherExpense GetOpeningBalance(string OpeningDate);
        OtherExpense InsertOtherExpense(OtherExpense otherExpense);
        OtherExpense UpdateOtherExpense(OtherExpense otherExpense);
        object DeleteOtherExpense(Guid ID, string UserName);
        OtherExpSummary GetOtherExpSummary(int month, int year, string Company);
        List<OtherExpense> GetExpenseTypeDetails(OtherExpense expObj);
        List<OtherExpense> GetBankWiseBalance(string Date);
        object Validate(OtherExpense _OtherexpenseObj);
        string GetValueFromSettings(SysSettings SysSettings);
        string UpdateValueInSettings(SysSettings SysSettings);
        List<OtherExpense> GetAllOtherExpenseByApprovalStatus(int? ApprovalStatus,string ExpenseDate);
        OtherExpense GetOtherExpenseByID(Guid ID);
        string ApproveOtherExpense(Guid ID);
        string PayOtherExpense(Guid ID, string CreatedBy);
        bool NotifyOtherExpense(Guid ID);
    }
}
