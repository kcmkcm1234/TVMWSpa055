using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ISupplierPaymentsRepository
    {
        List<SupplierPayments> GetAllSupplierPayments();
        SupplierPayments GetSupplierPaymentsByID(string ID);
        SupplierPayments InsertCustomerPayments(SupplierPayments _supplierPayObj);
        SupplierPayments UpdateCustomerPayments(SupplierPayments _supplierPayObj);
        object DeletePayments(Guid PaymentID, string UserName);
        SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj);
        SupplierPayments GetOutstandingAmountBySupplier(string SupplierID);

    }
}
