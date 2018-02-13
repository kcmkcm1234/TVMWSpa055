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
    public class PaymentFollowupRepository : IPaymentFollowupRepository
    {
        Settings settings = new Settings();
        Settings s = new Settings();
        AppConst Cobj = new AppConst();
        private IDatabaseFactory _databaseFactory;
        public PaymentFollowupRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region CustomerExpedityDetail
        public List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? toDate, string filter, string company, string customer,string outstanding)
        {
            List<CustomerExpeditingReport> customerExpeditingList = null;
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
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 50).Value = filter;
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 50).Value = company;
                        if (customer != "")
                            cmd.Parameters.Add("@Customer", SqlDbType.NVarChar, -1).Value = customer;
                        cmd.Parameters.Add("@Outstanding", SqlDbType.NVarChar, 50).Value = outstanding;
                        cmd.CommandText = "[Accounts].[CustomerExpeditingDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerExpeditingList = new List<CustomerExpeditingReport>();
                                while (sdr.Read())
                                {
                                    CustomerExpeditingReport customerExpeditingDetail = new CustomerExpeditingReport();
                                    {
                                       
                                        customerExpeditingDetail.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customerExpeditingDetail.CustomerName);
                                        customerExpeditingDetail.CustomerContactObj = new CustomerContactDetailsReport();
                                        customerExpeditingDetail.CustomerContactObj.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : customerExpeditingDetail.CustomerContactObj.ContactName);
                                        customerExpeditingDetail.ContactNo = (sdr["Mobile"].ToString() != "" ? ("☎" + sdr["Mobile"].ToString()) : "") +' '+
                                            (sdr["LandLine"].ToString() != "" ? (", ☎" + sdr["LandLine"].ToString()) : "") +' '+
                                            (sdr["OtherPhoneNos"].ToString() != "" ? (", ☎" + string.Join(", ",(sdr["OtherPhoneNos"].ToString().Split(',')))) : "");
                                        customerExpeditingDetail.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : customerExpeditingDetail.Mobile);
                                        customerExpeditingDetail.CompanyObj = new Companies();
                                        customerExpeditingDetail.CompanyObj.Name = (sdr["CompanyName1"].ToString() != "" ? sdr["CompanyName1"].ToString() : customerExpeditingDetail.CompanyObj.Name);
                                        customerExpeditingDetail.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : customerExpeditingDetail.InvoiceNo);
                                        customerExpeditingDetail.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : customerExpeditingDetail.InvoiceDate);
                                        customerExpeditingDetail.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : customerExpeditingDetail.Amount);
                                        customerExpeditingDetail.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? sdr["NoOfDays"].ToString() : customerExpeditingDetail.NoOfDays);
                                        customerExpeditingDetail.PaymentDueDate= (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : customerExpeditingDetail.PaymentDueDate);
                                        customerExpeditingDetail.CustomerObj = new Customer();
                                        customerExpeditingDetail.CustomerObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerExpeditingDetail.CustomerObj.ID);
                                    }
                                    customerExpeditingList.Add(customerExpeditingDetail);
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
            return customerExpeditingList;
        }
        #endregion CustomerExpedityDetail

        #region GetRecentFollowUps
        public List<FollowUp> GetRecentFollowUpCount(DateTime? Today)
        {
            List<FollowUp> followUpList = null;
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
                        cmd.CommandText = "[Accounts].[GetRecentFollowUpList]";
                        cmd.Parameters.Add("@Today", SqlDbType.Date).Value = Today;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                followUpList = new List<FollowUp>();
                                while (sdr.Read())
                                {
                                    FollowUp followUpObj = new FollowUp();
                                    {
                                        followUpObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : followUpObj.ID);
                                        followUpObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : followUpObj.CustomerID);
                                        followUpObj.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : followUpObj.ContactName);
                                        followUpObj.Company = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : followUpObj.Company);
                                        followUpObj.FollowUpDate = (sdr["FollowUpDate"].ToString() != "" ? DateTime.Parse(sdr["FollowUpDate"].ToString()).ToString(s.dateformat) : followUpObj.FollowUpDate);
                                        followUpObj.FollowUpTime = (sdr["FollowUpTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("hh:mm tt") : followUpObj.FollowUpTime);
                                        followUpObj.ContactNO = (sdr["ContactNO"].ToString() != "" ? sdr["ContactNO"].ToString() : followUpObj.ContactNO);
                                        followUpObj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : followUpObj.Remarks);
                                        followUpObj.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : followUpObj.Status);
                                    }
                                    followUpList.Add(followUpObj);
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

            return followUpList;
        }
        #endregion GetRecentFollowUps

        #region InsertFollowUp
        public FollowUp InsertFollowUp(FollowUp followupObj)
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
                        cmd.CommandText = "[Accounts].[InsertFollowUp]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FollowUpDate", SqlDbType.DateTime).Value = followupObj.FollowUpDate;
                        if (followupObj.HdnFollowUpTime == "" || followupObj.HdnFollowUpTime == null)
                        {
                            followupObj.HdnFollowUpTime = "10:00:00";
                        }
                        cmd.Parameters.Add("@FollowUpTime", SqlDbType.Time).Value = followupObj.HdnFollowUpTime;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = followupObj.CustomerID;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar, 150).Value = followupObj.Status;
                        cmd.Parameters.Add("@ContactName", SqlDbType.VarChar, 10).Value = followupObj.ContactName;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = followupObj.Remarks;
                        cmd.Parameters.Add("@ContactNO", SqlDbType.VarChar, 100).Value = followupObj.ContactNO;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = followupObj.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = followupObj.CommonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@InsertStatus", SqlDbType.SmallInt);
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
                        followupObj.
                                ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return followupObj;
        }
        #endregion InsertFollowUp

        #region UpdateFollowUp
        public FollowUp UpdateFollowUp(FollowUp followupObj)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[Accounts].[UpdateFollowUp]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = followupObj.ID;
                        cmd.Parameters.Add("@FollowUpDate", SqlDbType.DateTime).Value = followupObj.FollowUpDate;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = followupObj.CustomerID;
                        cmd.Parameters.Add("@FollowUpTime", SqlDbType.DateTime).Value = followupObj.HdnFollowUpTime;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar, 10).Value = followupObj.Status;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = followupObj.Remarks;
                        cmd.Parameters.Add("@ContactName", SqlDbType.VarChar, 150).Value = followupObj.ContactName;
                        cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar, 100).Value = followupObj.ContactNO;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = followupObj.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = followupObj.CommonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@UpdateStatus", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return followupObj;
        }
        #endregion UpdateFollowUp

        #region GetFollowupDetailsByFollowUpID

        public FollowUp GetFollowupDetailsByFollowUpID(Guid ID)
        {
            FollowUp followUpObj = null;
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
                        cmd.CommandText = "[Accounts].[GetFollowUpDetailsByFollowUpId]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                if (sdr.Read())
                                {
                                    followUpObj = new FollowUp();
                                    {
                                        followUpObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : followUpObj.ID);
                                        followUpObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : followUpObj.CustomerID);
                                        followUpObj.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : followUpObj.ContactName);
                                        followUpObj.ContactNO = (sdr["ContactNO"].ToString() != "" ? sdr["ContactNO"].ToString() : followUpObj.ContactNO);
                                        followUpObj.FollowUpDate = (sdr["FollowUpDate"].ToString() != "" ? DateTime.Parse(sdr["FollowUpDate"].ToString()).ToString(s.dateformat) : followUpObj.FollowUpDate);
                                        followUpObj.FollowUpTime = (sdr["FollowUpTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("hh:mm tt") : followUpObj.FollowUpTime);
                                        followUpObj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : followUpObj.Remarks);
                                        followUpObj.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : followUpObj.Status);
                                    }
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

            return followUpObj;
        }

        #endregion GetFollowupDetailsByFollowUpID

        #region DeleteFollowUp
        public object DeleteFollowUp(Guid ID)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[Accounts].[DeleteFollowUpDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(Cobj.DeleteFailure);
                    case "1":
                        return new
                        {
                            status = outputStatus.Value.ToString(),
                            Message = Cobj.DeleteSuccess
                        };


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return new
            {

            };

        }

        #endregion DeleteFollowUp

        #region FollowupList
        public List<FollowUp> GetFollowUpDetails(Guid customerID)
        {
            List<FollowUp> followUpList = null;
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
                        cmd.CommandText = "[Accounts].[GetFollowUpList]";
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerID;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                followUpList = new List<FollowUp>();
                                while (sdr.Read())
                                {
                                    FollowUp followUpObj = new FollowUp();
                                    {
                                        followUpObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : followUpObj.ID);
                                        followUpObj.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : followUpObj.CustomerID);
                                        followUpObj.ContactName = (sdr["ContactName"].ToString() != "" ? sdr["ContactName"].ToString() : followUpObj.ContactName);
                                        followUpObj.FollowUpDate = (sdr["FollowUpDate"].ToString() != "" ? DateTime.Parse(sdr["FollowUpDate"].ToString()).ToString(s.dateformat) : followUpObj.FollowUpDate);
                                        followUpObj.FollowUpTime = (sdr["FollowUpTime"].ToString() != "" ? DateTime.Parse(sdr["FollowUpTime"].ToString()).ToString("hh:mm tt") : followUpObj.FollowUpTime);
                                        followUpObj.ContactNO = (sdr["ContactNO"].ToString() != "" ? sdr["ContactNO"].ToString() : followUpObj.ContactNO);
                                        followUpObj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : followUpObj.Remarks);
                                        followUpObj.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : followUpObj.Status);
                                    }
                                    followUpList.Add(followUpObj);
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

            return followUpList;
        }
        #endregion FollowupList
    }
}