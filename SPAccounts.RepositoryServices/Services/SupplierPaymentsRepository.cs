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
        
        public SupplierPayments GetSupplierPaymentsByID(string ID)
        {
            SupplierPayments PaymentsObj = new SupplierPayments();
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[Accounts].[GetSupplierPaymentsDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    PaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : PaymentsObj.ID);
                                    PaymentsObj.PaidFromComanyCode = (sdr["PaidFromComanyCode"].ToString() != "" ? sdr["PaidFromComanyCode"].ToString() : PaymentsObj.PaidFromComanyCode);
                                    PaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : PaymentsObj.PaymentDateFormatted);
                                    PaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : PaymentsObj.PaymentRef);
                                    PaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : PaymentsObj.EntryNo);
                                    PaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : PaymentsObj.PaymentMode);
                                    PaymentsObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : PaymentsObj.Type);
                                    PaymentsObj.CreditID = (sdr["CreditID"].ToString() != "" ? Guid.Parse(sdr["CreditID"].ToString()) : PaymentsObj.CreditID);
                                    PaymentsObj.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : PaymentsObj.CreditNo);
                                    PaymentsObj.TotalPaidAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : PaymentsObj.TotalPaidAmt);
                                    PaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : PaymentsObj.AdvanceAmount);
                                    PaymentsObj.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : PaymentsObj.BankCode);
                                    PaymentsObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : PaymentsObj.DepWithdID);
                                    PaymentsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : PaymentsObj.GeneralNotes);
                                    PaymentsObj.supplierObj = new Supplier();
                                    PaymentsObj.supplierObj.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : PaymentsObj.supplierObj.ID);
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
            return PaymentsObj;

        }

        public SupplierPayments InsertCustomerPayments(SupplierPayments _supplierPayObj)
        {
            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.supplierObj.ID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _supplierPayObj.PaymentDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentMode;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.BankCode;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentRef;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaidFromComanyCode;
                        cmd.Parameters.Add("@TotalPaidAmt", SqlDbType.Decimal).Value = _supplierPayObj.TotalPaidAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _supplierPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _supplierPayObj.Type;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.CreditID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierPayObj.GeneralNotes;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _supplierPayObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierPayObj;  
        }

        public SupplierPayments UpdateCustomerPayments(SupplierPayments _supplierPayObj)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[UpdateSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_supplierPayObj.hdfSupplierID);
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _supplierPayObj.PaymentDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentMode;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = _supplierPayObj.hdfType;
                        cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.hdfCreditID;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.BankCode;
                        cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.DepWithdID;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaymentRef;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = _supplierPayObj.PaidFromComanyCode;
                        cmd.Parameters.Add("@TotalPaidAmt", SqlDbType.Decimal).Value = _supplierPayObj.TotalPaidAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = _supplierPayObj.AdvanceAmount;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierPayObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.UpdateFailure);
                    case "1":
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierPayObj;

        }

        public SupplierPayments InsertPaymentAdjustment(SupplierPayments _supplierPayObj)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[SupplierAdvanceAdjustment]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierPayObj.supplierObj.ID;
                        cmd.Parameters.Add("@DetailXml", SqlDbType.NVarChar, -1).Value = _supplierPayObj.DetailXml;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierPayObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierPayObj.CommonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierPayObj;

        }

        public object DeletePayments(Guid PaymentID, string UserName)
        {
            AppConst Cobj = new AppConst();
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[DeleteSupplierPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(Cobj.InsertFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { Message = Cobj.DeleteSuccess };

        }

        public SupplierPayments GetOutstandingAmountBySupplier(string SupplierID)
        {
            SupplierPayments PaymentsObj = new SupplierPayments();
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
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(SupplierID);
                        cmd.CommandText = "[Accounts].[GetOutstandingAmountBySupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    PaymentsObj.OutstandingAmount = (sdr["OutstandingAmount"].ToString() != "" ? sdr["OutstandingAmount"].ToString() : PaymentsObj.OutstandingAmount);
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
            return PaymentsObj;

        }

    }
}