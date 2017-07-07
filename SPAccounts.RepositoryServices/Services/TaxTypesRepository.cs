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
    public class TaxTypesRepository:ITaxTypesRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public TaxTypesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllTaxTypes
        public List<TaxTypes> GetAllTaxTypes()
        {
            List<TaxTypes> taxTypesList = null;
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
                        cmd.CommandText = "[Accounts].[GetTaxTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                taxTypesList = new List<TaxTypes>();
                                while (sdr.Read())
                                {
                                    TaxTypes _taxTypesObj = new TaxTypes();
                                    {
                                        _taxTypesObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _taxTypesObj.Code);
                                        _taxTypesObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _taxTypesObj.Description);
                                        _taxTypesObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : _taxTypesObj.Rate);
                                    }
                                    taxTypesList.Add(_taxTypesObj);
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

            return taxTypesList;
        }
        #endregion GetAllTaxTypes

        #region GetTaxTypeDetailsByCode
        public TaxTypes GetTaxTypeDetailsByCode(string Code)
        {
           TaxTypes _taxTypesObj = null;
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
                        cmd.CommandText = "[Accounts].[GetTaxTypeDetails]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _taxTypesObj = new TaxTypes();
                                        _taxTypesObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _taxTypesObj.Code);
                                        _taxTypesObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _taxTypesObj.Description);
                                        _taxTypesObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : _taxTypesObj.Rate);
                                    
                                }
                            }
                        }
                    }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _taxTypesObj;
        }
        #endregion GetTaxTypeDetailsByCode

        #region InsertTaxType
        public TaxTypes InsertTaxType(TaxTypes _taxTypesObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[InsertTaxType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _taxTypesObj.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = _taxTypesObj.Description;
                        cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = _taxTypesObj.Rate;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
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
                        _taxTypesObj.Code = outputCode.Value.ToString();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _taxTypesObj;
        }
        #endregion InsertTaxType

        #region UpdateTaxType
        public object UpdateTaxType(TaxTypes _taxTypesObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[UpdateTaxType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _taxTypesObj.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = _taxTypesObj.Description;
                        cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = _taxTypesObj.Rate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
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
                        _taxTypesObj.Code = outputStatus.Value.ToString();
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
        #endregion UpdateTaxType

        #region DeleteTaxType
        public object DeleteTaxType(string code)
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
                        cmd.CommandText = "[Accounts].[DeleteTaxType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = code;
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
        #endregion DeleteTaxType

    }
}