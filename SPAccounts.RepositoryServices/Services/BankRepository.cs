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
    public class BankRepository : IBankRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public BankRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }


        #region GetAllBank
        public List<Bank> GetAllBank()
        {
            List<Bank> bankList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllBanks]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bankList = new List<Bank>();
                                while (sdr.Read())
                                {
                                    Bank _bankObj = new Bank();
                                    {
                                        _bankObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _bankObj.Code);
                                        _bankObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _bankObj.Name);
                                        _bankObj.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : _bankObj.CompanyCode);
                                        _bankObj.Company = new Companies();
                                       _bankObj.Company.Name= (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _bankObj.Company.Name);
                                    }
                                    bankList.Add(_bankObj);
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

            return bankList;
        }
        #endregion GetAllBank

        //#region GetBankDetailsByCode
        //public Bank GetBankDetailsByCode(string Code)
        //{
        //    Bank bankObj = null;
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[Accounts].[GetBankDetailsByCode]";
        //                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value =Code;
        //                cmd.CommandType = CommandType.StoredProcedure;                     

        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if ((sdr != null) && (sdr.HasRows))
        //                        if (sdr.Read())
        //                        {
        //                            bankObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : bankObj.Code);
        //                            bankObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : bankObj.Name);
        //                            bankObj.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : bankObj.CompanyCode);

        //                        }
        //                }

        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return bankObj;
        //}
        //#endregion GetBankDetailsByCode

        #region GetBankDetailsByCode
        public Bank GetBankDetailsByCode(string Code)
        {
            Bank _bankObj = null;
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
                        cmd.CommandText = "[Accounts].[GetBankDetailsByCode]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _bankObj = new Bank();
                                    _bankObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _bankObj.Code);
                                    _bankObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _bankObj.Name);
                                    _bankObj.CompanyCode = (sdr["CompanyCode"].ToString() != "" ? sdr["CompanyCode"].ToString() : _bankObj.CompanyCode);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _bankObj;
        }
        #endregion GetBankDetailsByCode

        #region InsertBank
        public Bank InsertBank(Bank _bankObj)
        {
            try
            {
                SqlParameter outputStatus, outputCode = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertBank]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = _bankObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _bankObj.Name;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar,10).Value = _bankObj.CompanyCode;                        
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value =_bankObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _bankObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar,5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _bankObj.Code = outputCode.Value.ToString();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _bankObj;
        }
        #endregion InsertBank

        #region UpdateBank
        public object UpdateBank(Bank _bankObj)
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
                        cmd.CommandText = "[Accounts].[UpdateBank]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = _bankObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _bankObj.Name;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 10).Value = _bankObj.CompanyCode;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _bankObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _bankObj.commonObj.UpdatedDate;
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
                        _bankObj.Code = outputStatus.Value.ToString();
                        break;
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
                Status = outputStatus.Value.ToString(),
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion UpdateBank

        #region DeleteBank
        public object DeleteBank(string code)
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
                        cmd.CommandText = "[Accounts].[DeleteBank]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = code;                        
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
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = Cobj.DeleteSuccess
            };
        }
        #endregion DeleteBank
    }
}