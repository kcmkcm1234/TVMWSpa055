using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ISupplierPaymentsBusiness
    {
        List<SupplierPayments> GetAllSupplierPayments();
        List<SupplierPayments> GetAllPendingSupplierPayments();
        SupplierPayments GetSupplierPaymentsByID(string ID);
        List<SupplierPayments> GetSupplierInvoiceAdjustedByPaymentID(SupplierPayments SupObj);
       SupplierPayments ApprovedSupplierPayment(SupplierPayments SupObj);
        SupplierPayments InsertUpdatePayments(SupplierPayments _supplierPayObj);
        object DeletePayments(Guid PaymentID, string UserName);
        SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj);
        SupplierPayments GetOutstandingAmountBySupplier(string SupplierID);
        object ApprovedPayment(Guid PaymentID, string UserName,DateTime date);
        object Validate(SupplierPayments _supplierpayObj);
        void SendToFCM(string titleString, string descriptionString, Boolean isCommon, string CustomerID = "");
        SupplierPayments UpdateSupplierPaymentGeneralNotes(SupplierPayments supobj);
        object UpdateNotification(SupplierPayments _supplierpayObj);

    }
}
