﻿using SPAccounts.DataAccessObject.DTO;
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
        SupplierPayments GetSupplierPaymentsByID(string ID);
        SupplierPayments InsertUpdatePayments(SupplierPayments _supplierPayObj);
        object DeletePayments(Guid PaymentID, string UserName);
        SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj);
        SupplierPayments GetOutstandingAmountBySupplier(string SupplierID);
    }
}
