using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IDepositAndWithdrawalsBusiness
    {
        List<DepositAndWithdrawals> GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr);
        DepositAndWithdrawals GetDepositAndWithdrawalDetails(Guid ID);
        DepositAndWithdrawals GetTransferCashById(Guid TransferId);
        OutGoingCheques GetOutgoingChequeById(Guid ID);
        IncomingCheques GetIncomingChequeById(Guid ID);
        object InsertUpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object DeleteDepositandwithdrawal(Guid ID, string UserName);
        object DeleteTransferAmount(Guid TransferID, string UserName);
        object ClearCheque(string IDS,string date);
        List<DepositAndWithdrawals> GetUndepositedCheque(UndepositedChequeAdvanceSearch undepositedChequeAdvanceSearchObject);
        List<OutGoingCheques> GetOutGoingCheques(OutgoingChequeAdvanceSearch outGoingChequesAdvanceSearchObject);
        List<IncomingCheques> GetIncomingCheques(OutgoingChequeAdvanceSearch incomingChequesAdvanceSearchObject);
        object InsertUpdateOutgoingCheque(OutGoingCheques outGoingChequeObj);
        object InsertUpdateIncomingCheque(IncomingCheques incomingChequeObj);
        object DeleteOutgoingCheque(Guid ID);
        object DeleteIncomingCheque(Guid ID);
        string GetUndepositedChequeCount(string Date);
        object InsertUpdateTransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);
        object ClearChequeOut(string ID, string Date);
        List<DepositAndWithdrawals> GetAllWithdrawals();
        object ValidateChequeNo(OutGoingCheques outGoingChequeObj);
        object ValidateChequeNoIncomingCheque(IncomingCheques incomingChequeObj);
    }
}
