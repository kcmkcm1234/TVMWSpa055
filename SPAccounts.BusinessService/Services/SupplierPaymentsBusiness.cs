using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;

namespace SPAccounts.BusinessService.Services
{
    public class SupplierPaymentsBusiness: ISupplierPaymentsBusiness
    {
        private ISupplierPaymentsRepository _supplierPaymentsRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierPaymentsBusiness(ISupplierPaymentsRepository supplierPaymentsRepository, ICommonBusiness commonBusiness)
        {
            _supplierPaymentsRepository = supplierPaymentsRepository;
            _commonBusiness = commonBusiness;

        }

        public List<SupplierPayments> GetAllSupplierPayments()
        {
            List<SupplierPayments> supplierPayObj = null;
            supplierPayObj = _supplierPaymentsRepository.GetAllSupplierPayments();
            return supplierPayObj;
        }

        public SupplierPayments GetSupplierPaymentsByID(string ID)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments InsertUpdatePayments(SupplierPayments _supplierPayObj)
        {
            throw new NotImplementedException();
        }

        public object DeletePayments(Guid PaymentID, string UserName)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments GetOutstandingAmountBySupplier(string SupplierID)
        {
            throw new NotImplementedException();
        }
    }
}