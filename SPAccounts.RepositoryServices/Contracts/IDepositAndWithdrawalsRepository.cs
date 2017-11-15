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
        DepositAndWithdrawals GetTransferCashById(Guid TransferId);
        DepositAndWithdrawals InsertDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object UpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object DeleteDepositandwithdrawal(Guid ID, string UserName);
        object DeleteTransferAmount(Guid TransferID, string UserName);
        object ClearCheque(Guid ID,string date);
        List<DepositAndWithdrawals> GetUndepositedCheque(string FromDate, string ToDate);
        string GetUndepositedChequeCount(string Date);
        DepositAndWithdrawals TransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);
        object UpdateTransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);
    }
}
