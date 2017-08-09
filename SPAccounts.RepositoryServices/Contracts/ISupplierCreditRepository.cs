using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
  public interface ISupplierCreditRepository
  {
        List<SupplierCreditNote> GetAllSupplierCreditNotes();
        SupplierCreditNote InsertSupplierCreditNotes(SupplierCreditNote _supplierCreditNoteObj);
        object UpdateSupplierCreditNotes(SupplierCreditNote _supplierCreditNoteObj);
        SupplierCreditNote GetSupplierCreditNoteDetails(Guid ID);
        object DeleteSupplierCreditNote(Guid ID, string userName);
        List<CustomerCreditNotes> GetCreditNoteByPaymentID(Guid SupplierID, Guid PaymentID);
        List<SupplierCreditNote> GetCreditNoteBySupplier(Guid SupplierID);
    }
}
