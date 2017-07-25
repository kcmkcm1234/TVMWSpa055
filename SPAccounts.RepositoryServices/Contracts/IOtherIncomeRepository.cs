using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface IOtherIncomeRepository
    {
        List<OtherIncome> GetAllOtherIncome(string IncomeDate);
        OtherIncome GetOtherIncomeDetails(Guid ID);
        OtherIncome InsertOtherIncome(OtherIncome _otherIncomeObj, AppUA ua);
        object UpdateOtherIncome(OtherIncome _otherIncomeObj, AppUA ua);
        object DeleteOtherIncome(Guid ID);
    }
}
