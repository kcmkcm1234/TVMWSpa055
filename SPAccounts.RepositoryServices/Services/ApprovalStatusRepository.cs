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
    public class ApprovalStatusRepository: IApprovalStatusRepository
    {

        private IDatabaseFactory _databaseFactory;
        public ApprovalStatusRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllApprovalStatus
        public List<ApprovalStatus> GetAllApprovalStatus()
        {
            List<ApprovalStatus> StatusList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllApprovalStatus]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                StatusList = new List<ApprovalStatus>();
                                while (sdr.Read())
                                {
                                    ApprovalStatus _ApprovalObj = new ApprovalStatus();
                                    {
                                        _ApprovalObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _ApprovalObj.Code);
                                        _ApprovalObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _ApprovalObj.Description);
                                    }
                                    StatusList.Add(_ApprovalObj);
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

            return StatusList;
        }
        #endregion GetAllApprovalStatus

    }
}