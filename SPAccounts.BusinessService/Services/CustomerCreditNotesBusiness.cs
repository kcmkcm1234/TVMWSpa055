using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerCreditNotesBusiness : ICustomerCreditNotesBusiness
    {
        private ICustomerCreditNotesRepository _customerCreditNotesRepository;
        private ICommonBusiness _commonBusiness;

        public CustomerCreditNotesBusiness(ICustomerCreditNotesRepository customerCreditNotesRepository, ICommonBusiness commonBusiness)
        {
            _customerCreditNotesRepository = customerCreditNotesRepository;
            _commonBusiness = commonBusiness;
        }
        public List<CustomerCreditNotes> GetAllCustomerCreditNotes()
        {
            return _customerCreditNotesRepository.GetAllCustomerCreditNotes();
        }
        public CustomerCreditNotes GetCustomerCreditNoteDetails(Guid ID)
        {
            CustomerCreditNotes custCreditNotesObj = new CustomerCreditNotes();
            custCreditNotesObj = _customerCreditNotesRepository.GetCustomerCreditNoteDetails(ID);
            if(custCreditNotesObj!=null)
            {
                custCreditNotesObj.creditAmountFormatted= _commonBusiness.ConvertCurrency(custCreditNotesObj.CreditAmount, 2);
            }
            return custCreditNotesObj;
        }
        public object InsertUpdateCustomerCreditNote(CustomerCreditNotes _customerCreditNoteObj, AppUA ua)
        {
            object result = null;
            try
            {
                if (_customerCreditNoteObj.ID == null)
                {
                    result = _customerCreditNotesRepository.InsertCustomerCreditNotes(_customerCreditNoteObj, ua);
                }
                else
                {
                    result = _customerCreditNotesRepository.UpdateCustomerCreditNotes(_customerCreditNoteObj, ua);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteCustomerCreditNote(Guid ID)
        {
            object result = null;
            try
            {
                result = _customerCreditNotesRepository.DeleteCustomerCreditNotes(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<CustomerCreditNotes> GetCreditNoteByCustomer(Guid ID)
        {
            List<CustomerCreditNotes> custcreditlist = new List<CustomerCreditNotes>();
            custcreditlist = _customerCreditNotesRepository.GetCreditNoteByCustomer(ID);
            return custcreditlist;
        }
        public CustomerCreditNotes GetCreditNoteAmount(Guid CreditID,Guid CustomerID)
        {
            CustomerCreditNotes creditnote = new CustomerCreditNotes();
            List<CustomerCreditNotes> custcreditlist = new List<CustomerCreditNotes>();
            custcreditlist = _customerCreditNotesRepository.GetCreditNoteByCustomer(CustomerID);
            custcreditlist = custcreditlist.Where(m => m.ID == CreditID).ToList();
            creditnote = custcreditlist[0];
            return creditnote;
        }
    }
}