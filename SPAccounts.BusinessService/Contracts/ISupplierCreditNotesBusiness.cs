using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ISupplierCreditNotesBusiness
    {

        List<SupplierCreditNote> GetAllSupplierCreditNotes();
        SupplierCreditNote GetSupplierCreditNoteDetails(Guid ID);
        object InsertUpdateSupplierCreditNote(SupplierCreditNote _supplierCreditNoteObj);
        object DeleteSupplierCreditNote(Guid ID, string userName);
    }
}
