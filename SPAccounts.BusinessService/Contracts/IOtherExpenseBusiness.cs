using SPAccounts.DataAccessObject.DTO;
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
        List<ChartOfAccounts> GetAllAccountTypes(string accountType);
        List<Companies> GetAllCompanies();
        List<PaymentModes> GetAllPaymentModes();
        List<Bank> GetAllBankes();
        List<Employee> GetAllEmployees();
        OtherExpense InsertOtherExpense(OtherExpense otherExpense);
    }
}
