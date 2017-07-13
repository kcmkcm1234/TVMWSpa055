using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;

namespace SPAccounts.BusinessService.Services
{
    public class SupplierCreditBusines : ISupplierCreditBusines
    {
        ISupplierCreditRepository _supplierCreditRepository;
        public SupplierCreditBusines(ISupplierCreditRepository supplierCreditRepository)
        {
            _supplierCreditRepository = supplierCreditRepository;
        }

        public List<SupplierCreditNote> GetAllSupplierCreditNotes()
        {
            List<SupplierCreditNote> supplierCreditNoteList = null;
            try
            {
                supplierCreditNoteList=_supplierCreditRepository.GetAllSupplierCreditNotes();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return supplierCreditNoteList;
        }
    }
}