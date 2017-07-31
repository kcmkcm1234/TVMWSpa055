using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerInvoicesBusiness : ICustomerInvoicesBusiness
    {
        private ICustomerInvoicesRepository _customerInvoicesRepository;
        private ICommonBusiness _commonBusiness;
        public CustomerInvoicesBusiness(ICustomerInvoicesRepository customerInvoicesRepository, ICommonBusiness commonBusiness)
        {
            _customerInvoicesRepository = customerInvoicesRepository;
            _commonBusiness = commonBusiness;

        }

        public List<CustomerInvoice> GetAllCustomerInvoices()
        {
            try
            {
                return _customerInvoicesRepository.GetAllCustomerInvoices();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public CustomerInvoice GetCustomerInvoiceDetails(Guid ID)
        {
            return _customerInvoicesRepository.GetCustomerInvoiceDetails(ID);
        }
        public CustomerInvoiceSummary GetCustomerInvoicesSummary() {
            try
            {
                CustomerInvoiceSummary result= new CustomerInvoiceSummary();
                result= _customerInvoicesRepository.GetCustomerInvoicesSummary();
                if (result != null) {

                    result.OpenAmountFormatted = _commonBusiness.ConvertCurrency(result.OpenAmount, 2);
                    result.PaidAmountFormatted = _commonBusiness.ConvertCurrency(result.PaidAmount, 2);
                    result.OverdueAmountFormatted = _commonBusiness.ConvertCurrency(result.OverdueAmount, 2);

                }

                return result;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public CustomerInvoice InsertUpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua)
        {
            if(_customerInvoicesObj.ID!=null&& _customerInvoicesObj.ID!=Guid.Empty)
            {
                return _customerInvoicesRepository.UpdateInvoice(_customerInvoicesObj, ua);
            }
            else
            {
                return _customerInvoicesRepository.InsertInvoice(_customerInvoicesObj, ua);
            }
            
        }

        public List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID)
        {
            return _customerInvoicesRepository.GetOutStandingInvoices(PaymentID,CustID);
        }

        public CustomerInvoice GetCustomerAdvances(string ID)
        {
            return _customerInvoicesRepository.GetCustomerAdvances(ID);
        }

        public object DeleteInvoices(Guid ID, string UserName)
        {
            return _customerInvoicesRepository.DeleteInvoices(ID,UserName);
        }
        public List<CustomerInvoice> GetOutstandingCustomerInvoices()
        {
            try
            {
                return _customerInvoicesRepository.GetOutstandingCustomerInvoices();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<CustomerInvoice> GetOpeningCustomerInvoices()
        {
            try
            {
                return _customerInvoicesRepository.GetOpeningCustomerInvoices();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}