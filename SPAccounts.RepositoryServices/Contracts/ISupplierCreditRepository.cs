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
  }
}
