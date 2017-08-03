using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SPAccounts.RepositoryServices.Services
{
    public class SupplierPaymentsRepository: ISupplierPaymentsRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SupplierPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public object DeletePayments(Guid PaymentID, string UserName)
        {
            throw new NotImplementedException();
        }

        public List<SupplierPayments> GetAllSupplierPayments()
        {
            List<SupplierPayments> SupplerPaylist = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetAllSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplerPaylist = new List<SupplierPayments>();
                                while (sdr.Read())
                                {
                                    SupplierPayments PaymentsObj = new SupplierPayments();
                                    {
                                        PaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : PaymentsObj.ID);
                                        PaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.PaymentDateFormatted);
                                        PaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : PaymentsObj.PaymentRef);
                                        PaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : PaymentsObj.EntryNo);
                                        PaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : PaymentsObj.PaymentMode);
                                        PaymentsObj.TotalPaidAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : PaymentsObj.TotalPaidAmt);
                                        PaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : PaymentsObj.AdvanceAmount);
                                        PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                        PaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : PaymentsObj.CreditNo);
                                        PaymentsObj.supplierObj = new Supplier();
                                        PaymentsObj.supplierObj.CompanyName = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : PaymentsObj.supplierObj.CompanyName);
                                        PaymentsObj.supplierObj.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : PaymentsObj.supplierObj.ID);
                                        PaymentsObj.supplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : PaymentsObj.supplierObj.ContactPerson);
                                    };
                                    SupplerPaylist.Add(PaymentsObj);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SupplerPaylist;
        }

        public SupplierPayments GetOutstandingAmountBySupplier(string SupplierID)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments GetSupplierPaymentsByID(string ID)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments InsertCustomerPayments(SupplierPayments _supplierPayObj)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj)
        {
            throw new NotImplementedException();
        }

        public SupplierPayments UpdateCustomerPayments(SupplierPayments _supplierPayObj)
        {
            throw new NotImplementedException();
        }
    }
}