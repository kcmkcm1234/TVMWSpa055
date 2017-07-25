using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Reflection;

namespace SPAccounts.BusinessService.Services
{
    public class CustomerPaymentsBusiness: ICustomerPaymentsBusiness
    { 
        private ICustomerPaymentsRepository _customerPaymentsRepository;
        public CustomerPaymentsBusiness(ICustomerPaymentsRepository custPaymentRepository)
        {
            _customerPaymentsRepository = custPaymentRepository;
        }

        public List<CustomerPayments> GetAllCustomerPayments()
        {
            List<CustomerPayments> custPayObj = null;
            custPayObj = _customerPaymentsRepository.GetAllCustomerPayments();
            return custPayObj;
        }

        public CustomerPayments GetCustomerPaymentsByID(string ID)
        {
            CustomerPayments custPayObj = null;
            custPayObj = _customerPaymentsRepository.GetCustomerPaymentsByID(ID);
            return custPayObj;

        }

        public CustomerPayments InsertUpdatePayments(CustomerPayments _custPayObj)
        {
            if (_custPayObj.ID != null && _custPayObj.ID != Guid.Empty)
            {
                PaymentDetailsXMl(_custPayObj);
                return _customerPaymentsRepository.UpdateCustomerPayments(_custPayObj);
            }
            else
            {
                PaymentDetailsXMl(_custPayObj);
                return _customerPaymentsRepository.InsertCustomerPayments(_custPayObj);
            }
        }

        public CustomerPayments InsertPaymentAdjustment(CustomerPayments _custPayObj)
        { 
                PaymentDetailsXMl(_custPayObj);
                return _customerPaymentsRepository.InsertPaymentAdjustment(_custPayObj);
        }

        public void PaymentDetailsXMl(CustomerPayments CustPaymentObj)
        {
            string result = "<Details>";
            int totalRows = 0; 
            foreach (object some_object in CustPaymentObj.CustomerPaymentsDetail)
            {
                XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";
           
                CustPaymentObj.DetailXml = result;
           
        }
        private void XML(object some_object, ref string result, ref int totalRows)
        {
            var properties = GetProperties(some_object); 
            result = result + "<item ";  
            foreach (var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(some_object, null);
                result = result + " " + name + @"=""" + value + @""" ";
            }
            result = result + "></item>";
            totalRows = totalRows + 1;
        }
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        public object DeletePayments(Guid PaymentID,string UserName)
        {
            return _customerPaymentsRepository.DeletePayments(PaymentID, UserName);
        }
    }
}