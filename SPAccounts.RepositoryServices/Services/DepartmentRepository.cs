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
    public class DepartmentRepository : IDepartmentRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public DepartmentRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Department> GetAllDetpartments()
        {
            List<Department> departmentList = null;
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
                        cmd.CommandText = "[Accounts].[GetDepartments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                departmentList = new List<Department>();
                                while (sdr.Read())
                                {
                                    Department _departmentObj = new Department();
                                    {
                                        _departmentObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _departmentObj.Code);
                                        _departmentObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _departmentObj.Name);
                                        _departmentObj.commonObj = new Common();
                                        {
                                            _departmentObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _departmentObj.commonObj.CreatedBy);
                                            _departmentObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _departmentObj.commonObj.CreatedDate);
                                            _departmentObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : string.Empty);
                                        }
                                    }
                                    departmentList.Add(_departmentObj);
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

            return departmentList;
        }

        #region InsertDepartment
        public object InsertDepartment(Department _DepartmentObj)
        {
            try
            {
                SqlParameter outputStatus;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertDepartment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _DepartmentObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _DepartmentObj.Name;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _DepartmentObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _DepartmentObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }
                AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        //success
                      
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = Cobj.InsertSuccess
                        };
                      
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _DepartmentObj;
        }
        #endregion InsertDepartment

        #region UpdateDepartment
        public object UpdateDepartment(Department _DepartmentObj)
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
                        cmd.CommandText = "[Accounts].[UpdateDepartment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = _DepartmentObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = _DepartmentObj.Name;
                      
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _DepartmentObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _DepartmentObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }
                AppConst Cobj = new AppConst();
                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);

                    case "1":

                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = Cobj.UpdateSuccess
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
                Status = outputStatus.Value.ToString(),
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion UpdateEmployee

        #region DeleteDepartment
        public object DeleteDepartment(string Code)
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
                        cmd.CommandText = "[Accounts].[DeleteDepartment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
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
        #endregion DeleteDepartment
    }
}