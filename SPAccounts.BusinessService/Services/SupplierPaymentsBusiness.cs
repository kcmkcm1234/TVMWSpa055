using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class SupplierPaymentsBusiness
    {
        private ISupplierPaymentsRepository _supplierPaymentsRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierPaymentsBusiness(ISupplierPaymentsRepository supplierPaymentsRepository, ICommonBusiness commonBusiness)
        {
            _supplierPaymentsRepository = supplierPaymentsRepository;
            _commonBusiness = commonBusiness;

        }
    }
}