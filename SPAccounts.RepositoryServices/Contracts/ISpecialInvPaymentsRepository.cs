using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
  public  interface ISpecialInvPaymentsRepository
    {
        List<SpecialInvPayments> GetSpecialInvPayments(Guid PaymentID, Guid ID);
        SpecialInvPayments GetOutstandingSpecialAmountByCustomer(string ID);
        object Validate(SpecialInvPayments specialInvObj);
        SpecialInvPayments InsertSpecialInvPayments(SpecialInvPayments specialObj);
        SpecialInvPayments UpdateSpecialInvPayments(SpecialInvPayments _specialPayments);
       List<SpecialInvPayments> GetSpecialInvPaymentsDetails(Guid GroupID);
        List<SpecialInvPayments> GetAllSpecialInvPayments(SpecialPaymentsSearch SpecialPaymentsSearch);
        object DeleteSpecialPayments(Guid GroupID);
    }
}
