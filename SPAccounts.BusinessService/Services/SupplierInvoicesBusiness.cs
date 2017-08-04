using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class SupplierInvoicesBusiness:ISupplierInvoicesBusiness
    {
        private ISupplierInvoicesRepository _supplierInvoicesRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierInvoicesBusiness(ISupplierInvoicesRepository supplierInvoicesRepository, ICommonBusiness commonBusiness)
        {
            _supplierInvoicesRepository = supplierInvoicesRepository;
            _commonBusiness = commonBusiness;

        }
        public List<SupplierInvoices> GetAllSupplierInvoices()
        {
            return _supplierInvoicesRepository.GetAllSupplierInvoices();
        }
        public SupplierInvoiceSummary GetSupplierInvoicesSummary()
        {
            SupplierInvoiceSummary result = new SupplierInvoiceSummary();
            result = _supplierInvoicesRepository.GetSupplierInvoicesSummary();
            if (result != null)
            {

                result.OpenAmountFormatted = _commonBusiness.ConvertCurrency(result.OpenAmount, 2);
                result.PaidAmountFormatted = _commonBusiness.ConvertCurrency(result.PaidAmount, 2);
                result.OverdueAmountFormatted = _commonBusiness.ConvertCurrency(result.OverdueAmount, 2);

            }
            return result;
        }
        public SupplierInvoices InsertUpdateInvoice(SupplierInvoices _supplierInvoicesObj)
        {
            if (_supplierInvoicesObj.ID != null && _supplierInvoicesObj.ID != Guid.Empty)
            {
                return _supplierInvoicesRepository.UpdateInvoice(_supplierInvoicesObj);
            }
            else
            {
                return _supplierInvoicesRepository.InsertInvoice(_supplierInvoicesObj);
            }
        }
        public SupplierInvoices GetSupplierInvoiceDetails(Guid ID)
        {
            return _supplierInvoicesRepository.GetSupplierInvoiceDetails(ID);
        }

        public SupplierSummaryforMobile GetOutstandingSupplierInvoices()
        {
            SupplierSummaryforMobile supObj= new SupplierSummaryforMobile();
            supObj.supInvSumObj = new SupplierInvoiceSummaryformobile();
            try
            {
                decimal tmp = 0;
                supObj.SupInv=_supplierInvoicesRepository.GetOutstandingSupplierInvoices();
                if (supObj.SupInv == null)
                {
                    supObj.supInvSumObj.Amount = 0;
                    supObj.supInvSumObj.AmountFormatted = "0";
                    supObj.supInvSumObj.count = 0;
                }
                else
                {
                    foreach (SupplierInvoices m in supObj.SupInv)
                    {
                        tmp = tmp + m.BalanceDue;
                    }
                    supObj.supInvSumObj.Amount = tmp;
                    supObj.supInvSumObj.AmountFormatted = _commonBusiness.ConvertCurrency(tmp, 0);
                    supObj.supInvSumObj.count = supObj.SupInv.Count;
                }
            }
            
            catch (Exception)
            {

                throw;
            }
            return supObj;

        }

        public SupplierSummaryforMobile GetOpeningSupplierInvoices()
        {
            SupplierSummaryforMobile supObj = new SupplierSummaryforMobile();
            supObj.supInvSumObj = new SupplierInvoiceSummaryformobile();
            try
            {
                decimal tmp = 0;
                supObj.SupInv = _supplierInvoicesRepository.GetOpeningSupplierInvoices();
                if (supObj.SupInv == null)
                {
                    supObj.supInvSumObj.Amount = 0;
                    supObj.supInvSumObj.AmountFormatted = "0";
                    supObj.supInvSumObj.count = 0;
                }
                else
                {
                    foreach (SupplierInvoices m in supObj.SupInv)
                    {
                        tmp = tmp + m.BalanceDue;
                    }
                    supObj.supInvSumObj.Amount = tmp;
                    supObj.supInvSumObj.AmountFormatted = _commonBusiness.ConvertCurrency(tmp, 0);
                    supObj.supInvSumObj.count = supObj.SupInv.Count;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return supObj;
        }
        public object DeleteSupplierInvoice(Guid ID, string userName)
        {
            object result = null;
            try
            {
                result = _supplierInvoicesRepository.DeleteSupplierInvoice(ID, userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<SupplierInvoices> GetOutStandingInvoicesBySupplier(Guid PaymentID, Guid supplierID)
        {
            return _supplierInvoicesRepository.GetOutStandingInvoicesBySupplier(PaymentID, supplierID);
        }
    }
}