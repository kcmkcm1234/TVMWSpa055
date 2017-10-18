using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface ICustomerPaymentsRepository
    {
        List<CustomerPayments> GetAllCustomerPayments();
        CustomerPayments GetCustomerPaymentsByID(string ID);
        CustomerPayments InsertCustomerPayments(CustomerPayments _custPayObj);
        CustomerPayments UpdateCustomerPayments(CustomerPayments _custPayObj);
        object DeletePayments(Guid PaymentId,string UserName);
        object Validate(CustomerPayments _customerpayObj);
        CustomerPayments GetOutstandingAmountByCustomer(string CustomerID);
        CustomerPayments InsertPaymentAdjustment(CustomerPayments _custPayObj);
    }
}
