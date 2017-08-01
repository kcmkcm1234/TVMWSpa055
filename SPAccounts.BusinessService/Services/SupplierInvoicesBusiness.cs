﻿using SPAccounts.BusinessService.Contracts;
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
            return _supplierInvoicesRepository.GetSupplierInvoicesSummary();
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
    }
}