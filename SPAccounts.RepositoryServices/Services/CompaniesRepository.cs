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
    public class CompaniesRepository:ICompaniesRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public CompaniesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Companies> GetAllCompanies()
        {
            List<Companies> companyList = null;
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
                        cmd.CommandText = "[Accounts].[GetCompanies]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                companyList = new List<Companies>();
                                while (sdr.Read())
                                {
                                    Companies _companyObj = new Companies();
                                    {
                                        _companyObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _companyObj.Code);
                                        _companyObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _companyObj.Name);
                                        _companyObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _companyObj.ShippingAddress);
                                        _companyObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _companyObj.BillingAddress);
                                        _companyObj.ApproverID = (sdr["ApproverID"].ToString() != "" ? Guid.Parse(sdr["ApproverID"].ToString()) : _companyObj.ApproverID);

                                    }
                                    companyList.Add(_companyObj);
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

            return companyList;
        }

        #region GetCompanyDetailsByCode
        public Companies GetCompanyDetailsByCode(string Code)
        {
            Companies _companyObj = null;
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
                        cmd.CommandText = "[Accounts].[GetCompanyDetailsByCode]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _companyObj = new Companies();
                                    _companyObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _companyObj.Code);
                                    _companyObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _companyObj.Name);
                                    _companyObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _companyObj.BillingAddress);
                                    _companyObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _companyObj.ShippingAddress);
                                    _companyObj.ApproverID = (sdr["ApproverID"].ToString() != "" ? Guid.Parse(sdr["ApproverID"].ToString()) : _companyObj.ApproverID);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _companyObj;
        }
        #endregion GetCompanyDetailsByCode


        #region InsertCompany
        public Companies InsertCompany(Companies _companyObj)
        {
            //try
            //{
            //    SqlParameter outputStatus, outputCode = null;
            //    using (SqlConnection con = _databaseFactory.GetDBConnection())
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            if (con.State == ConnectionState.Closed)
            //            {
            //                con.Open();
            //            }
            //            cmd.Connection = con;
            //            cmd.CommandText = "[Accounts].[InsertCompany]";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = _companyObj.Code;
            //            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _companyObj.Name;
            //            cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar, 10).Value = _companyObj.BillingAddress;
            //            cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar, 10).Value = _companyObj.ShippingAddress;
            //            cmd.Parameters.Add("@ApproverID", SqlDbType.VarChar, 10).Value = _companyObj.ApproverID;

            //            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _companyObj.commonObj.CreatedBy;
            //            cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _companyObj.commonObj.CreatedDate;
            //            outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
            //            outputStatus.Direction = ParameterDirection.Output;
            //            outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
            //            outputCode.Direction = ParameterDirection.Output;
            //            cmd.ExecuteNonQuery();


            //        }
            //    }

            //    switch (outputStatus.Value.ToString())
            //    {
            //        case "0":
            //            AppConst Cobj = new AppConst();
            //            throw new Exception(Cobj.InsertFailure);
            //        case "1":
            //            _companyObj.Code = outputCode.Value.ToString();
            //            break;
            //        default:
            //            break;
            //    }

            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return _companyObj;
        }
        #endregion InsertCompany

        #region UpdateCompany
        public Companies UpdateCompany(Companies _companyObj)
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
                        cmd.CommandText = "[Accounts].[UpdateCompany]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = _companyObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _companyObj.Name;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar, 10).Value = _companyObj.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar, 10).Value = _companyObj.ShippingAddress;
                        cmd.Parameters.Add("@ApproverID", SqlDbType.UniqueIdentifier).Value = _companyObj.ApproverID;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _companyObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _companyObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);
                    case "1":
                        _companyObj.Code = _companyObj.Code;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _companyObj;
        }
        #endregion UpdateCompany
    }
}