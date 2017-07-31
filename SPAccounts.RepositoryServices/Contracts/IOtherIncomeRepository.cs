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
        List<OtherIncome> GetAllOtherIncome(string IncomeDate,string DefaultDate);
        OtherIncome GetOtherIncomeDetails(Guid ID);
        OtherIncome InsertOtherIncome(OtherIncome _otherIncomeObj);
        object UpdateOtherIncome(OtherIncome _otherIncomeObj);
        object DeleteOtherIncome(Guid ID, string userName);
    }
}
