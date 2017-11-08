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

        public List<CustomerInvoice> GetAllCustomerInvoices(DateTime? FromDate, DateTime? ToDate, string Customer, string InvoiceType, string Company, string Status, string Search)
        {
            try
            {
                List<CustomerInvoice> custlist = new List<CustomerInvoice>();
                custlist= _customerInvoicesRepository.GetAllCustomerInvoices(FromDate,ToDate,Customer,InvoiceType,Company,Status,Search);
                if(custlist!=null)
                custlist = custlist.Where(C => C.InvoiceType == "RB").ToList();
                return custlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoice> GetAllCustomerInvoicesForSA(DateTime? FromDate, DateTime? ToDate, string Customer, string InvoiceType, string Company, string Status, string Search)
        {
            try
            {
                return _customerInvoicesRepository.GetAllCustomerInvoices(FromDate, ToDate, Customer, InvoiceType, Company, Status, Search);
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
                result= _customerInvoicesRepository.GetAllCustomerInvoices( null,null,null,null,null,null,null);
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
        public CustomerInvoiceSummary GetCustomerInvoicesSummary(bool IsInternal)
        {
            try
            {
                CustomerInvoiceSummary result = new CustomerInvoiceSummary();
                result = _customerInvoicesRepository.GetCustomerInvoicesSummary(IsInternal);
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
        public CustomerInvoicesSummaryForMobile GetCustomerInvoicesByDateWise(CustomerInvoice CusmObj)
        {
            CustomerInvoicesSummaryForMobile cusumObj = new CustomerInvoicesSummaryForMobile();
            cusumObj.CustInvSumObj = new InvoiceSummaryformobile();
            try
            {
                decimal tmp = 0;

                cusumObj.CustInv = _customerInvoicesRepository.GetCustomerInvoicesByDateWise(CusmObj);
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


        public List<CustomerInvoice> GetAllSpecialPayments(Guid InvoiceID)
        {
            return _customerInvoicesRepository.GetAllSpecialPayments(InvoiceID);
        }

        public CustomerInvoice GetSpecialPaymentsDetails(Guid ID)
        {
            return _customerInvoicesRepository.GetSpecialPaymentsDetails(ID);
        }

        public CustomerInvoice InsertUpdateSpecialPayments(CustomerInvoice _customerInvoicesObj, AppUA ua)
        {

            if (_customerInvoicesObj.SpecialPayObj.ID != null && _customerInvoicesObj.SpecialPayObj.ID != Guid.Empty)
            {
                return _customerInvoicesRepository.UpdateSpecialPayments(_customerInvoicesObj, ua);
            }
            else
            {
                return _customerInvoicesRepository.InsertSpecialPayments(_customerInvoicesObj, ua);
            }

        }

        public object DeleteSpecialPayments(Guid ID)
        {
            return _customerInvoicesRepository.DeleteSpecialPayments(ID);
        }

        public CustomerInvoice SpecialPaymentSummary(Guid InvoiceID)
        {
            CustomerInvoice CI = new CustomerInvoice();
            CI= _customerInvoicesRepository.SpecialPaymentSummary(InvoiceID);
            
            return CI;
        }

        public CustomerInvoiceAgeingSummary GetCustomerInvoicesAgeingSummary() {
            return _customerInvoicesRepository.GetCustomerInvoicesAgeingSummary();
        }
    }
}