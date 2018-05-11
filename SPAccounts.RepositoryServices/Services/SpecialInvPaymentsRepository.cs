using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class SpecialInvPaymentsRepository : ISpecialInvPaymentsRepository
    {

        private IDatabaseFactory _databaseFactory;

        public SpecialInvPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SpecialInvPayments> GetSpecialInvPayments(Guid PaymentID, Guid ID)
        {
            List<SpecialInvPayments> SpecialObjList = null; 
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.CommandText = "[Accounts].[GetSpecialInvPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SpecialObjList = new List<SpecialInvPayments>();
                                while (sdr.Read())
                                {
                                    SpecialInvPayments SpecialObj = new SpecialInvPayments();
                                    {
                                      //  SpecialObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SpecialObj.ID);
                                        SpecialObj.specialDetailObj = new SpecialInvPaymentsDetail();
                                        SpecialObj.ID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : SpecialObj.ID);
                                        SpecialObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : SpecialObj.InvoiceNo);
                                        SpecialObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SpecialObj.InvoiceDate);
                                        SpecialObj.companiesObj = new Companies();
                                        SpecialObj.companiesObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : SpecialObj.companiesObj.Name);
                                        SpecialObj.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SpecialObj.PaymentDueDate);

                                        SpecialObj.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["InvoiceAmount"].ToString()) : SpecialObj.InvoiceAmount);
                                        SpecialObj.specialDetailObj.PaidAmount = (sdr["PreviousPaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PreviousPaidAmount"].ToString()) : SpecialObj.specialDetailObj.PaidAmount);
                                        SpecialObj.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SpecialObj.BalanceDue);
                                        SpecialObj.specialDetailObj.CurrentAmount = (sdr["EditAmount"].ToString() != "" ? Decimal.Parse(sdr["EditAmount"].ToString()) : SpecialObj.specialDetailObj.CurrentAmount);
                                    }
                                    SpecialObjList.Add(SpecialObj); 
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
            return SpecialObjList;

        }

        public SpecialInvPayments GetOutstandingSpecialAmountByCustomer(string ID)
        {
            SpecialInvPayments SpecialPaysObj = new SpecialInvPayments();
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

                        cmd.CommandText = "[Accounts].[GetOutstandingSpecialAmountByCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SpecialPaysObj.InvoiceOutstanding = (sdr["InvoiceOutstanding"].ToString() != "" ? sdr["InvoiceOutstanding"].ToString() : SpecialPaysObj.InvoiceOutstanding);
                                    SpecialPaysObj.BalanceOutstanding = (sdr["BalanceOutstanding"].ToString() != "" ? sdr["BalanceOutstanding"].ToString() : SpecialPaysObj.BalanceOutstanding);
                                   

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
            return SpecialPaysObj;
        }

        public object Validate(SpecialInvPayments specialObj)
        {
            AppConst appCust = new AppConst();
            SqlParameter outputStatus = null;
            SqlParameter outputStatus1 = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[ValidateSpecialInvPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = specialObj.PaymentRef;
                        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value=specialObj.ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus1 = cmd.Parameters.Add("@message", SqlDbType.VarChar, 100);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputStatus1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {

                return new { message = ex.ToString(), Status = -1 };
            }

            return new { message = outputStatus1.Value.ToString(), status = outputStatus.Value };

        }

        public SpecialInvPayments InsertSpecialInvPayments(SpecialInvPayments specialObj)
        {
            try
            {
                SqlParameter outputStatus, outputID,outputGroupID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertSpecialInvPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                       // cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = specialObj.InvoiceID;
                        cmd.Parameters.Add("@PaymentDate",SqlDbType.DateTime).Value=specialObj.PaymentDate;
                       // cmd.Parameters.Add("@PaidAmount",SqlDbType.Decimal).Value=specialObj.PaidAmount;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar,10).Value = specialObj.PaymentMode;
                        cmd.Parameters.Add("@RefBank", SqlDbType.VarChar,50).Value = specialObj.RefBank;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = specialObj.ChequeDate;
                        cmd.Parameters.Add("@Paymentref", SqlDbType.VarChar, 20).Value = specialObj.PaymentRef;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.NVarChar, -1).Value = specialObj.DetailXml;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = specialObj.Remarks;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = specialObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = specialObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputGroupID = cmd.Parameters.Add("@GroupIDOut", SqlDbType.UniqueIdentifier);
                        outputGroupID.Direction = ParameterDirection.Output;
                        //outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        //outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                      //  specialObj.ID = new Guid(outputID.Value.ToString());
                        specialObj.GroupID = new Guid(outputGroupID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return specialObj;
        }
        public SpecialInvPayments UpdateSpecialInvPayments(SpecialInvPayments _specialPayments)
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
                        cmd.CommandText = "[Accounts].[UpdateSpecialInvPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                      //  cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = _specialPayments.specialDetailObj.InvoiceID;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _specialPayments.ID;
                        cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = _specialPayments.GroupID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = _specialPayments.PaymentDate;
                     //   cmd.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = _specialPayments.specialDetailObj.PaidAmount;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = _specialPayments.Remarks;

                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = _specialPayments.PaymentMode;
                        cmd.Parameters.Add("@RefBank", SqlDbType.VarChar, 50).Value = _specialPayments.RefBank;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _specialPayments.ChequeDate;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 20).Value = _specialPayments.PaymentRef;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.NVarChar, -1).Value = _specialPayments.DetailXml;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _specialPayments.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _specialPayments.commonObj.UpdatedDate;
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
            return _specialPayments;
        }
        public object DeleteSpecialPayments(Guid GroupID)
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
                        cmd.CommandText = "[Accounts].[DeleteSpecialInvPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = GroupID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(Cobj.DeleteFailure);
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
        public List<SpecialInvPayments> GetSpecialInvPaymentsDetails(Guid GroupID)
        {
            List<SpecialInvPayments> SPObjList = null;
                //    Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetSpecialInvPaymentDetails]";
                        cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = GroupID;
                        //cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = InvoiceID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                               SPObjList = new List<SpecialInvPayments>();
                                while (sdr.Read())
                                {
                                    SpecialInvPayments SPList = new SpecialInvPayments();
                                    {

                                        SPList.PaymentID = (sdr["PaymentID"].ToString() != "" ? Guid.Parse(sdr["PaymentID"].ToString()) : SPList.PaymentID);
                                        SPList.CustID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SPList.CustID);
                                        SPList.ID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : SPList.ID);                                    
                                        SPList.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : SPList.Remarks);
                                        SPList.specialDetailObj = new SpecialInvPaymentsDetail();
                                        SPList.specialDetailObj.PaidAmount = (sdr["Amount"].ToString() != "" ? Decimal.Parse(sdr["Amount"].ToString()) : SPList.specialDetailObj.PaidAmount);
                                        SPList.paymentDateFormatted = (sdr["PayDate"].ToString() != "" ? DateTime.Parse(sdr["PayDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SPList.paymentDateFormatted);
                                        SPList.chequeDateFormatted = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SPList.chequeDateFormatted);
                                        SPList.RefBank = (sdr["RefBank"].ToString() != "" ? sdr["RefBank"].ToString() : SPList.RefBank);
                                        SPList.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : SPList.PaymentRef);
                                        SPList.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : SPList.PaymentMode);


                                        SPList.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : SPList.GroupID);
                                       // SPList.ID = (sdr["PaymentID"].ToString() != "" ? Guid.Parse(sdr["PaymentID"].ToString()) : SPList.ID);
                                        SPList.specialDetailObj = new SpecialInvPaymentsDetail();
                                        SPList.specialDetailObj.InvoiceID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : SPList.specialDetailObj.InvoiceID);
                                        SPList.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : SPList.InvoiceNo);
                                        SPList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SPList.InvoiceDate);
                                        SPList.companiesObj = new Companies();
                                        SPList.companiesObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : SPList.companiesObj.Name);
                                        SPList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SPList.PaymentDueDate);

                                        SPList.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["InvoiceAmount"].ToString()) : SPList.InvoiceAmount);
                                        SPList.specialDetailObj.PaidAmount = (sdr["PreviousPaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PreviousPaidAmount"].ToString()) : SPList.specialDetailObj.PaidAmount);
                                        SPList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : SPList.BalanceDue);
                                        SPList.specialDetailObj.CurrentAmount = (sdr["Amount"].ToString() != "" ? Decimal.Parse(sdr["Amount"].ToString()) : SPList.specialDetailObj.CurrentAmount);
                                    }
                                    SPObjList.Add(SPList);
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

            return SPObjList;

        }


        public List<SpecialInvPayments> GetAllSpecialInvPayments(SpecialPaymentsSearch specialPaymentsSearch)
        {
            List<SpecialInvPayments> SpecialObjList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllSpecialInvPayments]";
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = specialPaymentsSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = specialPaymentsSearch.ToDate;
                        cmd.Parameters.Add("@CustomerCode", SqlDbType.NVarChar, 50).Value = specialPaymentsSearch.Customer;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 50).Value = specialPaymentsSearch.PaymentMode;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 50).Value = specialPaymentsSearch.Company;
                        cmd.Parameters.Add("@search", SqlDbType.NVarChar, 250).Value = specialPaymentsSearch.Search;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SpecialObjList = new List<SpecialInvPayments>();
                                while (sdr.Read())
                                {
                                    SpecialInvPayments SpecialObj = new SpecialInvPayments();
                                    {
                                        //SpecialObj.ID = (sdr["PaymentID"].ToString() != "" ? Guid.Parse(sdr["PaymentID"].ToString()) : SpecialObj.ID);
                                        // SpecialObj.specialDetailObj = new SpecialInvPaymentsDetail();
                                        //SpecialObj.specialDetailObj.InvoiceID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : SpecialObj.specialDetailObj.InvoiceID);
                                        //  SpecialObj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : SpecialObj.InvoiceNo);
                                        // SpecialObj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SpecialObj.InvoiceDate);
                                        //  SpecialObj.companiesObj = new Companies();
                                        //  SpecialObj.companiesObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : SpecialObj.companiesObj.Name);
                                        SpecialObj.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : SpecialObj.GroupID);
                                        SpecialObj.paymentDateFormatted = (sdr["PayDate"].ToString() != "" ? DateTime.Parse(sdr["PayDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : SpecialObj.paymentDateFormatted);

                                       SpecialObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? (sdr["PaymentRef"].ToString()) : SpecialObj.PaymentRef);
                                        SpecialObj.specialDetailObj = new SpecialInvPaymentsDetail();
                                        SpecialObj.specialDetailObj.PaidAmount = (sdr["SumAmount"].ToString() != "" ? Decimal.Parse(sdr["SumAmount"].ToString()) : SpecialObj.specialDetailObj.PaidAmount);
                                        SpecialObj.customerObj = new Customer();
                                        SpecialObj.customerObj.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : SpecialObj.customerObj.CompanyName);
                                       SpecialObj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : SpecialObj.Remarks);
                                    }
                                    SpecialObjList.Add(SpecialObj);
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
            return SpecialObjList;

        }
    }
}