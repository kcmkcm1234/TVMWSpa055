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
    }
}