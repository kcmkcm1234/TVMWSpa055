using SPAccounts.DataAccessObject.DTO;
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
        Bank InsertBank(Bank _bankObj, AppUA ua);
        List<Bank> GetBankDetailsByCode(string Code);
        object UpdateBank(Bank _bankObj, AppUA ua);
        object DeleteBank(string code);
    }
}
