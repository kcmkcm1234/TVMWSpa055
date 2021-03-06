﻿using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface IBankRepository
    {
        List<Bank> GetAllBank();
        object InsertBank(Bank _bankObj);
        Bank GetBankDetailsByCode(string Code);
        object UpdateBank(Bank _bankObj);
        object DeleteBank(string code);
    }
}
