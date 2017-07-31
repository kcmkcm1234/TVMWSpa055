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
        ICommonBusiness _commonBusinessRepository;
        public SupplierCreditBusines(ISupplierCreditRepository supplierCreditRepository,ICommonBusiness commonBusinessRepository)
        {
            _supplierCreditRepository = supplierCreditRepository;
            _commonBusinessRepository = commonBusinessRepository;
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

        public SupplierCreditNote GetSupplierCreditNoteDetails(Guid ID)
        {
            SupplierCreditNote supplierCreditNoteObj = new SupplierCreditNote();
            supplierCreditNoteObj = _supplierCreditRepository.GetSupplierCreditNoteDetails(ID);
            if (supplierCreditNoteObj != null)
            {
                supplierCreditNoteObj.creditAmountFormatted = _commonBusinessRepository.ConvertCurrency(supplierCreditNoteObj.Amount, 2);
                supplierCreditNoteObj.adjustedAmountFormatted = _commonBusinessRepository.ConvertCurrency(supplierCreditNoteObj.Amount,2);
            }
            return supplierCreditNoteObj;
        }

        public object InsertUpdateSupplierCreditNote(SupplierCreditNote _supplierCreditNoteObj)
        {
            object result = null;
            try
            {
                if (_supplierCreditNoteObj.ID == Guid.Empty)
                {
                    result = _supplierCreditRepository.InsertSupplierCreditNotes(_supplierCreditNoteObj);
                }
                else
                {
                    result = _supplierCreditRepository.UpdateSupplierCreditNotes(_supplierCreditNoteObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object DeleteSupplierCreditNote(Guid ID, string userName)
        {
            object result = null;
            try
            {
                result = _supplierCreditRepository.DeleteSupplierCreditNote(ID,userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}