using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface ICustomerCreditNotesRepository
    {
        List<CustomerCreditNotes> GetAllCustomerCreditNotes();
        List<CustomerCreditNotes> GetCreditNoteByCustomer(Guid ID);
        List<CustomerCreditNotes> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID);
        CustomerCreditNotes InsertCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj);
        object UpdateCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj);
        object DeleteCustomerCreditNotes(Guid ID, string userName);
        CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID);
    }
}
