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
        List<DepositAndWithdrawals> GetUndepositedCheque(UndepositedChequeAdvanceSearch undepositedChequeAdvanceSearchObject);
        List<OutGoingCheques> GetOutGoingCheques(OutgoingChequeAdvanceSearch advanceSearchObject);
        string GetUndepositedChequeCount(string Date);
        DepositAndWithdrawals TransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);
        object UpdateTransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);
        object ClearChequeOut(Guid ID, string Date);
        List<DepositAndWithdrawals> GetAllWithdrawals();
        OutGoingCheques InsertOutgoingCheques(OutGoingCheques outGoingChequeObj);
        object UpdateOutgoingCheques(OutGoingCheques outGoingChequeObj);
        object DeleteOutgoingCheque(Guid ID);
        OutGoingCheques GetOutgoingChequeById(Guid ID);
        object ValidateChequeNo(OutGoingCheques outGoingChequeObj);
    }
}
