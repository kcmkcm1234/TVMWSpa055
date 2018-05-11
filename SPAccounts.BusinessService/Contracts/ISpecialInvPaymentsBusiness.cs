using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
  public  interface ISpecialInvPaymentsBusiness
    {
        List<SpecialInvPayments> GetSpecialInvPayments(Guid PaymentID, Guid ID);
        // List<SpecialInvPayments> GetSpecialInvPayments(Guid ID);
        SpecialInvPayments GetOutstandingSpecialAmountByCustomer(string ID);
        SpecialInvPayments InsertUpdateSpecialInvPayments(SpecialInvPayments specialObj);
        object Validate(SpecialInvPayments SpecialInvObj);
       List<SpecialInvPayments> GetSpecialInvPaymentsDetails(Guid GroupID);
        List<SpecialInvPayments> GetAllSpecialInvPayments(SpecialPaymentsSearch specialPaymentsSearch);
        object DeleteSpecialPayments(Guid GroupID);
    }
}
