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
        CustomerCreditNotes InsertCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj, AppUA ua);
        object UpdateCustomerCreditNotes(CustomerCreditNotes _customerCreditNotesObj, AppUA ua);
        object DeleteCustomerCreditNotes(Guid ID);
        CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID);
    }
}
