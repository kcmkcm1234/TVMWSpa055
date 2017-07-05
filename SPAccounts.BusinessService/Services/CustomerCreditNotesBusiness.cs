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

        public CustomerCreditNotesBusiness(ICustomerCreditNotesRepository customerCreditNotesRepository)
        {
            _customerCreditNotesRepository = customerCreditNotesRepository;
        }
        public List<CustomerCreditNotes> GetAllCustomerCreditNotes()
        {
            return _customerCreditNotesRepository.GetAllCustomerCreditNotes();
        }
    }
}