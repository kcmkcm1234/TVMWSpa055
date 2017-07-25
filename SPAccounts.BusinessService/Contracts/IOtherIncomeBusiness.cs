using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IOtherIncomeBusiness
    {
        List<OtherIncome> GetAllOtherIncome(string IncomeDate);
        OtherIncome GetOtherIncomeDetails(Guid ID);
        object InsertUpdateOtherIncome(OtherIncome _otherIncomeObj, AppUA ua);
        object DeleteOtherIncome(Guid ID);
    }
}
