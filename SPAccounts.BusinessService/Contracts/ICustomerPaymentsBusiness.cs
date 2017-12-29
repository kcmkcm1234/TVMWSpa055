using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface ICustomerPaymentsBusiness
    {
        List<CustomerPayments> GetAllCustomerPayments(CustomerPaymentsSearch customerPaymentsSearch);
        CustomerPayments GetCustomerPaymentsByID(string ID);
        CustomerPayments InsertUpdatePayments(CustomerPayments _custPayObj);
        object DeletePayments(Guid PaymentID,string UserName);
        object Validate(CustomerPayments _customerpayObj);

        CustomerPayments InsertPaymentAdjustment(CustomerPayments _custPayObj);
        CustomerPayments GetOutstandingAmountByCustomer(string CustomerID);




    }
}
