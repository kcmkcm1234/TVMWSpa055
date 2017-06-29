using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICommonBusiness
    {
        string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true);
       
    }
}
