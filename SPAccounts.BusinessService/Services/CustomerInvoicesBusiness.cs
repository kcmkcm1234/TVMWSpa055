using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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
                List<CustomerInvoice> custlist = new List<CustomerInvoice>();
                custlist= _customerInvoicesRepository.GetAllCustomerInvoices();
                custlist = custlist.Where(C => C.InvoiceType == "RB").ToList();
                return custlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoice> GetAllCustomerInvoicesForSA()
        {
            try
            {
                return _customerInvoicesRepository.GetAllCustomerInvoices();
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public List<CustomerInvoice> GetAllCustomerInvociesByCustomerID(Guid CustomerID)
        {
            try
            {
                List<CustomerInvoice> result = new List<CustomerInvoice>();
                result= _customerInvoicesRepository.GetAllCustomerInvoices();
                if (result!=null)
                    result = result.Where(c => c.customerObj.ID == CustomerID && c.InvoiceType == "RB").ToList(); 
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public CustomerInvoice GetCustomerInvoiceDetails(Guid ID)
        {
            return _customerInvoicesRepository.GetCustomerInvoiceDetails(ID);
        }
        public CustomerInvoiceSummary GetCustomerInvoicesSummaryForSA() {
            try
            {
                CustomerInvoiceSummary result= new CustomerInvoiceSummary();
                result= _customerInvoicesRepository.GetCustomerInvoicesSummaryForSA();
                if (result != null) {

                    result.OpenAmountFormatted = _commonBusiness.ConvertCurrency(result.OpenAmount, 2);
                    result.PaidAmountFormatted = _commonBusiness.ConvertCurrency(result.PaidAmount, 2);
                    result.OverdueAmountFormatted = _commonBusiness.ConvertCurrency(result.OverdueAmount, 2);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CustomerInvoiceSummary GetCustomerInvoicesSummary()
        {
            try
            {
                CustomerInvoiceSummary result = new CustomerInvoiceSummary();
                result = _customerInvoicesRepository.GetCustomerInvoicesSummary();
                if (result != null)
                {

                    result.OpenAmountFormatted = _commonBusiness.ConvertCurrency(result.OpenAmount, 2);
                    result.PaidAmountFormatted = _commonBusiness.ConvertCurrency(result.PaidAmount, 2);
                    result.OverdueAmountFormatted = _commonBusiness.ConvertCurrency(result.OverdueAmount, 2);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
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
        public CustomerInvoicesSummaryForMobile GetOutstandingCustomerInvoices(CustomerInvoice CusObj)
        {
            CustomerInvoicesSummaryForMobile cusumObj = new CustomerInvoicesSummaryForMobile();
            cusumObj.CustInvSumObj = new InvoiceSummaryformobile();
            try
            {
                decimal tmp = 0;

                cusumObj.CustInv = _customerInvoicesRepository.GetOutstandingCustomerInvoices(CusObj);
                if (cusumObj.CustInv == null)
                {
                    cusumObj.CustInvSumObj.Amount = 0;
                    cusumObj.CustInvSumObj.AmountFormatted = "0";
                    cusumObj.CustInvSumObj.count = 0;
                }
                else
                {
                    foreach (CustomerInvoice m in cusumObj.CustInv)
                    {
                        tmp = tmp + m.BalanceDue;
                    }
                    cusumObj.CustInvSumObj.Amount = tmp;
                    cusumObj.CustInvSumObj.AmountFormatted = _commonBusiness.ConvertCurrency(tmp, 0);
                    cusumObj.CustInvSumObj.count = cusumObj.CustInv.Count;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return cusumObj;
        }

        public CustomerInvoicesSummaryForMobile GetOpeningCustomerInvoices()
        {

            CustomerInvoicesSummaryForMobile cusumObj = new CustomerInvoicesSummaryForMobile();
            cusumObj.CustInvSumObj = new InvoiceSummaryformobile();
            try
            {
                decimal tmp = 0;
                cusumObj.CustInv = _customerInvoicesRepository.GetOpeningCustomerInvoices();
                if (cusumObj.CustInv == null)
                {
                    cusumObj.CustInvSumObj.Amount = 0;
                    cusumObj.CustInvSumObj.AmountFormatted = "0";
                    cusumObj.CustInvSumObj.count = 0;
                }
                else
                {
                    foreach (CustomerInvoice m in cusumObj.CustInv)
                    {
                        tmp = tmp + m.BalanceDue;
                    }
                    cusumObj.CustInvSumObj.Amount = tmp;
                    cusumObj.CustInvSumObj.AmountFormatted = _commonBusiness.ConvertCurrency(tmp, 0);
                    cusumObj.CustInvSumObj.count = cusumObj.CustInv.Count;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return cusumObj;

        }
    }
}