using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface IDepositAndWithdrawalsRepository
    {
        List<DepositAndWithdrawals> GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr);
        DepositAndWithdrawals GetDepositAndWithdrawalDetails(Guid ID);
        DepositAndWithdrawals InsertDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object UpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object ClearCheque(Guid ID,string date);
    }
}
