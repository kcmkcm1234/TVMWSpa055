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
        List<CustomerCreditNotes> GetCreditNoteByCustomer(Guid ID);
        List<CustomerCreditNotes> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID);
        CustomerCreditNotes GetCreditNoteAmount(Guid CreditID, Guid CustomerID);
        CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID);
        object InsertUpdateCustomerCreditNote(CustomerCreditNotes _customerCreditNoteObj);
        object DeleteCustomerCreditNote(Guid ID,string userName);
    }
}
