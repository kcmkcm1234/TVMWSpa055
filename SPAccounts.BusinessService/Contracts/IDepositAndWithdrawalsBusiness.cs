﻿using SPAccounts.DataAccessObject.DTO;
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
        object InsertUpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj);
        object DeleteDepositandwithdrawal(Guid ID, string UserName);
        object DeleteTransferAmount(Guid TransferID, string UserName);
        object ClearCheque(string IDS,string date);
        List<DepositAndWithdrawals> GetUndepositedCheque(string FromDate, string ToDate);
        List<OutGoingCheques> GetOutGoingCheques(OutgoingChequeAdvanceSearch outGoingChequesAdvanceSearchObject);
        object InsertUpdateOutgoingCheque(OutGoingCheques outGoingChequeObj);
        object DeleteOutgoingCheque(Guid ID);
        string GetUndepositedChequeCount(string Date);
        object InsertUpdateTransferAmount(DepositAndWithdrawals _depositAndWithdrwalObj);

    }
}
