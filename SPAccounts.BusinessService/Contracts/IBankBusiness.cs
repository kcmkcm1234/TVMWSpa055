using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface IBankBusiness
    {
        List<Bank> GetAllBanks();
        object InsertUpdateBank(Bank bankObj, AppUA ua);
        List<Bank> GetBankDetailsByCode(string Code);
        object DeleteBank(string Code);
    }
}
