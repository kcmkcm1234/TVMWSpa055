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
    public class AccountHeadGroupRepository:IAccountHeadGroupRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public AccountHeadGroupRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<AccountHeadGroup> GetAllAccountHeadGroup()
        {
            List<AccountHeadGroup> accountHeadGroupList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllAccountHeadGroup]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountHeadGroupList = new List<AccountHeadGroup>();
                                while (sdr.Read())
                                {
                                    AccountHeadGroup _accountObj = new AccountHeadGroup();
                                    {
                                        _accountObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _accountObj.ID);
                                        _accountObj.GroupName = (sdr["GroupName"].ToString() != "" ? (sdr["GroupName"].ToString()) : _accountObj.GroupName);
                                        _accountObj.AccountHeads = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : _accountObj.AccountHeads);
                                    }
                                    accountHeadGroupList.Add(_accountObj);
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

            return accountHeadGroupList;
        }

        public List<AccountHeadGroup> GetDisabledCodeForAccountHeadGroup(string ID)
        {
            List<AccountHeadGroup> accountHeadGroupList = null;
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
                        cmd.CommandText = "[Accounts].[GetDisabledAccountCodeForAccountHeadGroup]";
                        if (ID != null)
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                accountHeadGroupList = new List<AccountHeadGroup>();
                                while (sdr.Read())
                                {
                                    AccountHeadGroup _accountObj = new AccountHeadGroup();
                                    {
                                        _accountObj.IsGrouped = (sdr["IsGrouped"].ToString() != "" ? bool.Parse(sdr["IsGrouped"].ToString()) : _accountObj.IsGrouped);
                                        _accountObj.AccountHeads = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _accountObj.AccountHeads);
                                    }
                                    accountHeadGroupList.Add(_accountObj);
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

            return accountHeadGroupList;
        }

        public AccountHeadGroup InsertAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[InsertAccountHeadGroup]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 50).Value = accountHeadGroup.GroupName;
                        cmd.Parameters.Add("@AccountHeads", SqlDbType.VarChar, 250).Value = accountHeadGroup.AccountHeads;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = accountHeadGroup.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = accountHeadGroup.commonObj.CreatedDate;
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
                        accountHeadGroup.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return accountHeadGroup;
        }

        public AccountHeadGroup UpdateAccountHeadGroup(AccountHeadGroup accountHeadGroup, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[UpdateAccountHeadGroup]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = accountHeadGroup.ID;
                        cmd.Parameters.Add("@GroupName", SqlDbType.VarChar, 50).Value = accountHeadGroup.GroupName;
                        cmd.Parameters.Add("@AccountHeads", SqlDbType.VarChar, 250).Value = accountHeadGroup.AccountHeads;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = accountHeadGroup.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = accountHeadGroup.commonObj.UpdatedDate;
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
                        // _customerInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return accountHeadGroup;
        }



        #region GetAccountHeadGroupDetailsByID
        public AccountHeadGroup GetAccountHeadGroupDetailsByID(Guid ID)
        {
            AccountHeadGroup accountHeadGroup = new AccountHeadGroup();
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
                        cmd.CommandText = "[Accounts].[GetAccountHeadGroupDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    accountHeadGroup.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : accountHeadGroup.ID);
                                    accountHeadGroup.GroupName = (sdr["GroupName"].ToString() != "" ? (sdr["GroupName"].ToString()) : accountHeadGroup.GroupName);
                                    accountHeadGroup.AccountHeads = (sdr["AccountHead"].ToString() != "" ? sdr["AccountHead"].ToString() : accountHeadGroup.AccountHeads);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return accountHeadGroup;
        }
        #endregion GetAccountHeadGroupDetailsByID


        public object DeleteAccountHeadGroup(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteAccountHeadGroup]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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

        #region GetAllEmployeeTypes
        public List<AccountHeadGroup> GetAllGroupName()
        {
            List<AccountHeadGroup> groupTypeList = null;
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
                        cmd.CommandText = "[Accounts].[GetGroupNameTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                groupTypeList = new List<AccountHeadGroup>();
                                while (sdr.Read())
                                {
                                    AccountHeadGroup groupType = new AccountHeadGroup()
                                    {
                                     ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) :Guid.Empty),
                                    GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : string.Empty)
                                    };

                                    groupTypeList.Add(groupType);
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
            return groupTypeList;
        }
        #endregion GetAllEmployeeTypes
    }
}