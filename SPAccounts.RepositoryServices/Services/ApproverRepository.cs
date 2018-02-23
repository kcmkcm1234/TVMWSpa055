using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace SPAccounts.RepositoryServices.Services
{
    public class ApproverRepository: IApproverRepository
    {
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public ApproverRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Approver> GetPreviousApprovalsInfo(Approver approver)
        {
            List<Approver> approvedlist = null;
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
                        cmd.CommandText = "[Accounts].[GetPreviousApprovalsInfo]";
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = approver.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = approver.ToDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                approvedlist = new List<Approver>();
                                while (sdr.Read())
                                {
                                    Approver _approval = new Approver();
                                    {
                                        _approval.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _approval.ID);
                                        _approval.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : string.Empty);
                                        _approval.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.dateformat) : _approval.Date);
                                        _approval.ApprovalDate = (sdr["ApprovalDate"].ToString() != "" ? DateTime.Parse(sdr["ApprovalDate"].ToString()).ToString(settings.dateformat) : _approval.ApprovalDate);
                                        _approval.Company = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : string.Empty);
                                        _approval.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : _approval.PaymentMode);
                                        _approval.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _approval.Amount);
                                        _approval.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _approval.Type);
                                       
                                    }

                                    approvedlist.Add(_approval);
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
            return approvedlist;
        }
    }
}