using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
   public interface ICustomerCreditNotesBusiness
    {
        List<CustomerCreditNotes> GetAllCustomerCreditNotes();
        CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID);
        object InsertUpdateCustomerCreditNote(CustomerCreditNotes _customerCreditNoteObj, AppUA ua);
        object DeleteCustomerCreditNote(Guid ID);
    }
}
