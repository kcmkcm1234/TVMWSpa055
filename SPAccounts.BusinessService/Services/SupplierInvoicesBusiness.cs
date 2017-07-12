using SPAccounts.BusinessService.Contracts;
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
    }
}