using SPAccounts.BusinessService.Contracts;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Reflection;
using System.Net;
using System.IO;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Text;

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
        public List<SupplierPayments> GetAllPendingSupplierPayments()
        {
            List<SupplierPayments> supplierPendingList = _supplierPaymentsRepository.GetAllPendingSupplierPayments();
            return supplierPendingList;
        }

        public SupplierPayments GetSupplierPaymentsByID(string ID)
        {
            SupplierPayments PayObj = null;
            PayObj = _supplierPaymentsRepository.GetSupplierPaymentsByID(ID);
            return PayObj;
        }
        public List<SupplierPayments> GetSupplierInvoiceAdjustedByPaymentID(SupplierPayments SupObj)
        {
            List<SupplierPayments> PaymentObj = _supplierPaymentsRepository.GetSupplierInvoiceAdjustedByPaymentID(SupObj);
            return PaymentObj;
        }
        public SupplierPayments ApprovedSupplierPayment(SupplierPayments SupObj)
        {
            SupplierPayments PaymentObj = _supplierPaymentsRepository.ApprovedSupplierPayment(SupObj);
            return PaymentObj;
        }
        
        public SupplierPayments InsertUpdatePayments(SupplierPayments _supplierPayObj)
        {
            if (_supplierPayObj.ID != null && _supplierPayObj.ID != Guid.Empty)
            {
                PaymentDetailsXMl(_supplierPayObj);
                return _supplierPaymentsRepository.UpdateSupplierPayments(_supplierPayObj);
            }
            else
            {
                PaymentDetailsXMl(_supplierPayObj);
                return _supplierPaymentsRepository.InsertSupplierPayments(_supplierPayObj);
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
            decimal temp = Decimal.Parse(PayObj.OutstandingAmount);
            PayObj.OutstandingAmount= _commonBusiness.ConvertCurrency(temp,0);
            return PayObj;
        }

        public object ApprovedPayment(Guid PaymentID, string UserName, DateTime date)
        {
            return _supplierPaymentsRepository.ApprovedPayment(PaymentID, UserName,date);
        }


        #region Notification message to Cloud messaging system
        /// <summary>
        /// Function to communicate with Firebase Cloud Messaging system by Google, for sending app notifications
        /// </summary>
        /// <param name="titleString">Title of notification</param>
        /// <param name="descriptionString">Description of notification</param>
        /// <param name="isCommon">Specify whether the notification is common for all app users or a specific</param>

        public void SendToFCM(string titleString, string descriptionString, Boolean isCommon, string CustomerID = "")
        {
            //Validation

            if (titleString == "" || titleString == null)
                throw new Exception("No title");
            if (descriptionString == "" || descriptionString == null)
                throw new Exception("No description");
            //Sending notification through Firebase Cluod Messaging
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                string to_String = "";
                if (isCommon)
                    to_String = "/topics/common";

                else
                    to_String = "/topics/" + CustomerID;
                var objNotification = new
                {
                    to = to_String,

                    data = new
                    {
                        title = titleString,
                        body = descriptionString,
                        sound = "default",

                    }
                };
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonNotificationFormat = js.Serialize(objNotification);
                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                //Put here the Server key from Firebase
                string FCMServerKey = ConfigurationManager.AppSettings["FCMServerKey"].ToString();
                tRequest.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                //Put here the Sender ID from Firebase
                string FCMSenderID = ConfigurationManager.AppSettings["FCMSenderID"].ToString();
                tRequest.Headers.Add(string.Format("Sender: id={0}", FCMSenderID));

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();
                                tReader.Close();
                                dataStream.Close();
                                tResponse.Close();

                                if (!responseFromFirebaseServer.Contains("message_id"))//Doesn't contain message_id means some error occured
                                    throw new Exception(responseFromFirebaseServer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Notification message to Cloud messaging system

        public object Validate(SupplierPayments _supplierpayObj)
        {
            return _supplierPaymentsRepository.Validate(_supplierpayObj);
        }

        
        public SupplierPayments UpdateSupplierPaymentGeneralNotes(SupplierPayments _supplierPayObj)
            { 
                return _supplierPaymentsRepository.UpdateSupplierPaymentGeneralNotes(_supplierPayObj);
            }
    }
}