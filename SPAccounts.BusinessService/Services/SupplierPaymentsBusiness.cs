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
            SupplierPayments PayObj = null;
            PayObj = _supplierPaymentsRepository.GetSupplierPaymentsByID(ID);
            return PayObj;
        } 
     
        public SupplierPayments InsertUpdatePayments(SupplierPayments _supplierPayObj)
        {
            if (_supplierPayObj.ID != null && _supplierPayObj.ID != Guid.Empty)
            {
                PaymentDetailsXMl(_supplierPayObj);
                return _supplierPaymentsRepository.UpdateCustomerPayments(_supplierPayObj);
            }
            else
            {
                PaymentDetailsXMl(_supplierPayObj);
                return _supplierPaymentsRepository.InsertCustomerPayments(_supplierPayObj);
            }         
        }

        public SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj)
        {
            PaymentDetailsXMl(_supplierPayObj);
            return _supplierPaymentsRepository.InsertPaymentAdjustment(_supplierPayObj);
        }


        public void PaymentDetailsXMl(SupplierPayments PaymentObj)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in PaymentObj.supplierPaymentsDetail)
            {
                XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            PaymentObj.DetailXml = result;

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



        public object DeletePayments(Guid PaymentID, string UserName)
        {
            return _supplierPaymentsRepository.DeletePayments(PaymentID, UserName);
        }

     
        public SupplierPayments GetOutstandingAmountBySupplier(string SupplierID)
        {
            SupplierPayments PayObj = _supplierPaymentsRepository.GetOutstandingAmountBySupplier(SupplierID);
            //decimal temp = Decimal.Parse(PayObj.OutstandingAmount);
            //PayObj.OutstandingAmount= _commonBusiness.ConvertCurrency(temp,0);
            return PayObj;
        }
    }
}