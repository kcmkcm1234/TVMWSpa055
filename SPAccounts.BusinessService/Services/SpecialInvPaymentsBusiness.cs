using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using SPAccounts.RepositoryServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class SpecialInvPaymentsBusiness: ISpecialInvPaymentsBusiness
    {

        private ISpecialInvPaymentsRepository _SpecialInvPaymentsRepository;
        private ICommonBusiness _commonBusiness;
        public SpecialInvPaymentsBusiness(ISpecialInvPaymentsRepository specialInvPaymentsRepository, ICommonBusiness commonBusiness)
        {
            _SpecialInvPaymentsRepository = specialInvPaymentsRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SpecialInvPayments> GetSpecialInvPayments(Guid PaymentID, Guid ID)
        {

          
            List <SpecialInvPayments> spclPayObj = null;
            spclPayObj= _SpecialInvPaymentsRepository.GetSpecialInvPayments(PaymentID,ID);
            return spclPayObj;

        }
        public SpecialInvPayments GetOutstandingSpecialAmountByCustomer(string ID)
        {
            SpecialInvPayments SpecialObj = _SpecialInvPaymentsRepository.GetOutstandingSpecialAmountByCustomer(ID);
            decimal temp = Decimal.Parse(SpecialObj.InvoiceOutstanding);
            SpecialObj.InvoiceOutstanding = _commonBusiness.ConvertCurrency(temp, 0);
            return SpecialObj;
        }
        public object  Validate(SpecialInvPayments SpecialInvObj)
        {
          return _SpecialInvPaymentsRepository.Validate(SpecialInvObj);
        }
        public SpecialInvPayments InsertUpdateSpecialInvPayments(SpecialInvPayments specialObj)
        {
            if(specialObj.ID!=null && specialObj.ID!=Guid.Empty)
            {
                PaymentDetailsXML(specialObj);
                return _SpecialInvPaymentsRepository.UpdateSpecialInvPayments(specialObj);
               
            }
            else
            {
                PaymentDetailsXML(specialObj);
                return _SpecialInvPaymentsRepository.InsertSpecialInvPayments(specialObj);
            }

        }
        public object DeleteSpecialPayments(Guid GroupID)
        {
            return _SpecialInvPaymentsRepository.DeleteSpecialPayments(GroupID);
        }
        public void PaymentDetailsXML(SpecialInvPayments specialObj)
        {
            string result = "<Details>";
            int totalRows = 0;
         //   List<SpecialInvPayments> loopObj = specialObj.specialList;
            foreach (object some_object in specialObj.specialList)
            {
                XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";
            specialObj.DetailXml = result;
        }
        private void XML(object some_object, ref String result, ref int totalRows)
        {
            var properties = GetProperties(some_object);
            result = result + "<Item ";
            foreach(var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(some_object,null);
                result = result + "" + name + @"=""" + value + @""" ";


            }
            result = result + "></Item>";
            totalRows = totalRows + 1;
        }
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        public List<SpecialInvPayments> GetSpecialInvPaymentsDetails(Guid GroupID)
        {
            return _SpecialInvPaymentsRepository.GetSpecialInvPaymentsDetails(GroupID);
        }

        public List<SpecialInvPayments> GetAllSpecialInvPayments(SpecialPaymentsSearch SpecialPaymentsSearch)
        {
            return _SpecialInvPaymentsRepository.GetAllSpecialInvPayments(SpecialPaymentsSearch);
        }
    }
}