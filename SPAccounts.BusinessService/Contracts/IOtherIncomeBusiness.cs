﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IOtherIncomeBusiness
    {
        List<OtherIncome> GetAllOtherIncome(string IncomeDate,string DefaultDate);
        OtherIncome GetOtherIncomeDetails(Guid ID);
        //object InsertUpdateOtherIncome(OtherIncome _otherIncomeObj);
        object DeleteOtherIncome(Guid ID, string userName);
        object Validate(OtherIncome _otherincome);
        OtherIncome InsertOtherIncome(OtherIncome _otherIncomeObj);
        OtherIncome UpdateOtherIncome(OtherIncome _otherIncomeObj);
    }
}
