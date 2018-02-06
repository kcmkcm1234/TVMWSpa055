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
    public class EmployeeRepository : IEmployeeRepository
    {
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public EmployeeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllEmployees
        public List<Employee> GetAllEmployees(string filter)
         {
            List<Employee> employeeList = null;
            
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
                        cmd.CommandText = "[Accounts].[GetAllEmployees]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Filter", SqlDbType.NVarChar,10).Value = filter;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeList = new List<Employee>();
                                while (sdr.Read())
                                {
                                    Employee employeeObj = new Employee();
                                    {
                                        employeeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employeeObj.ID);
                                        employeeObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employeeObj.Code);
                                        employeeObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employeeObj.Name);
                                        employeeObj.Department = (sdr["Department"].ToString() != "" ? sdr["Department"].ToString() : employeeObj.Department);
                                        employeeObj.EmployeeCategory = (sdr["EmployeeCategory"].ToString() != "" ? sdr["EmployeeCategory"].ToString() : employeeObj.EmployeeCategory);
                                        employeeObj.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employeeObj.MobileNo);
                                        employeeObj.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employeeObj.Address);
                                        employeeObj.employeeTypeObj = new EmployeeType()
                                        {
                                            Code = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : string.Empty),
                                            Name = (sdr["EmployeeType"].ToString() != "" ? sdr["EmployeeType"].ToString() : string.Empty)
                                        };
                                        employeeObj.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employeeObj.ImageURL);
                                      
                                        employeeObj.companies = new Companies()
                                        {
                                            Code = (sdr["CompanyID"].ToString() != "" ? (sdr["CompanyID"].ToString()) : string.Empty),
                                            Name = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : string.Empty)
                                        };

                                        employeeObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employeeObj.GeneralNotes);


                                        employeeObj.commonObj = new Common()
                                        {
                                            CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.dateformat) : string.Empty)
                                        };
                                        employeeObj.IsActive = (sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : employeeObj.IsActive);
                                        
                                    }

                                    employeeList.Add(employeeObj);
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
            return employeeList;
        }
        #endregion GetAllEmployees

        #region GetEmployeeDetails
        public Employee GetEmployeeDetails(Guid ID)
        {
            Employee employeeObj = new Employee();
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
                        cmd.CommandText = "[Accounts].[GetEmployeeDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {                                    
                                    employeeObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employeeObj.ID);
                                    employeeObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employeeObj.Code);
                                    employeeObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employeeObj.Name);
                                    employeeObj.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employeeObj.MobileNo);
                                    employeeObj.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employeeObj.Address);
                                    employeeObj.Department = (sdr["Department"].ToString() != "" ? sdr["Department"].ToString() :employeeObj.Department);
                                    employeeObj.EmployeeCategory = (sdr["EmployeeCategory"].ToString() != "" ? sdr["EmployeeCategory"].ToString() :employeeObj.EmployeeCategory);
                                    employeeObj.EmployeeType = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : employeeObj.EmployeeType);
                                    employeeObj.companyID = (sdr["CompanyID"].ToString() != "" ? sdr["CompanyID"].ToString() : employeeObj.companyID);
                                    employeeObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employeeObj.GeneralNotes);
                                    employeeObj.commonObj = new Common();
                                    employeeObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : employeeObj.commonObj.CreatedBy);
                                    employeeObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.dateformat) : employeeObj.commonObj.CreatedDateString);
                                    employeeObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : employeeObj.commonObj.CreatedDate);
                                    employeeObj.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : employeeObj.commonObj.UpdatedBy);
                                    employeeObj.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : employeeObj.commonObj.UpdatedDate);
                                    employeeObj.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(settings.dateformat) : employeeObj.commonObj.UpdatedDateString);
                                    employeeObj.IsActive = (sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : employeeObj.IsActive);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return employeeObj;
        }
        #endregion GetEmployeeDetails

        #region GetAllEmployeeTypes
        public List<EmployeeType> GetAllEmployeeTypes()
        {
            List<EmployeeType> employeeTypeList = null;
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
                        cmd.CommandText = "[Accounts].[GetEmployeeTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeTypeList = new List<EmployeeType>();
                                while (sdr.Read())
                                {
                                    EmployeeType employeeType = new EmployeeType()
                                    {
                                        Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : string.Empty),
                                        Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : string.Empty)
                                    };

                                    employeeTypeList.Add(employeeType);
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
            return employeeTypeList;
        }
        #endregion GetAllEmployeeTypes

        #region GetAllDepartments
        public List<EmployeeType> GetAllDepartment()
        {
            List<EmployeeType> employeeTypeList = null;
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
                        cmd.CommandText = "[Accounts].[GetDepartmentTypes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeTypeList = new List<EmployeeType>();
                                while (sdr.Read())
                                {
                                    EmployeeType employeeType = new EmployeeType()
                                    {
                                        Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : string.Empty),
                                        Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : string.Empty)
                                    };

                                    employeeTypeList.Add(employeeType);
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
            return employeeTypeList;
        }
        #endregion GetAllDepartments


        #region GetAllCategory
        public List<EmployeeType> GetAllCategory()
        {
            List<EmployeeType> employeeTypeList = null;
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
                        cmd.CommandText = "[Accounts].[GetEmployeeCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeTypeList = new List<EmployeeType>();
                                while (sdr.Read())
                                {
                                    EmployeeType employeeType = new EmployeeType()
                                    {
                                        Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : string.Empty),
                                        Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : string.Empty)
                                    };

                                    employeeTypeList.Add(employeeType);
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
            return employeeTypeList;
        }
        #endregion GetAllCategory
        #region InsertEmployee
        public Employee InsertEmployee(Employee employeeObj)
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
                        cmd.CommandText = "[Accounts].[InsertEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,10).Value = employeeObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = employeeObj.Name;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = employeeObj.MobileNo;
                        cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 100).Value = employeeObj.Department;
                        cmd.Parameters.Add("@EmployeeCategory", SqlDbType.NVarChar, 100).Value = employeeObj.EmployeeCategory;
                        cmd.Parameters.Add("@Address", SqlDbType.NVarChar, -1).Value = employeeObj.Address;                       
                        cmd.Parameters.Add("@EmpType", SqlDbType.VarChar,5).Value = employeeObj.EmployeeType;                        
                        cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar, 10).Value = employeeObj.companyID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = employeeObj.GeneralNotes; 
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = employeeObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = employeeObj.commonObj.CreatedDate;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employeeObj.IsActive;
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
                        employeeObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return employeeObj;
        }
        #endregion InsertEmployee

        #region UpdateEmployee
        public object UpdateEmployee(Employee employeeObj)
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
                        cmd.CommandText = "[Accounts].[UpdateEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = employeeObj.ID;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = employeeObj.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = employeeObj.Name;
                        cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 100).Value = employeeObj.Department;
                        cmd.Parameters.Add("@EmployeeCategory", SqlDbType.NVarChar, 100).Value = employeeObj.EmployeeCategory;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = employeeObj.MobileNo;
                        cmd.Parameters.Add("@Address", SqlDbType.NVarChar, -1).Value = employeeObj.Address;
                        cmd.Parameters.Add("@EmpType", SqlDbType.VarChar, 5).Value = employeeObj.EmployeeType;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar, 10).Value = employeeObj.companyID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = employeeObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = employeeObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = employeeObj.commonObj.UpdatedDate;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employeeObj.IsActive;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
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
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion UpdateEmployee

        #region DeleteEmployee
        public object DeleteEmployee(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteEmployee]";
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
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = Cobj.DeleteSuccess
            };
        }


        #endregion DeleteEmployee


        public List<EmployeeCategory> GetAllEmployeeCategories()
        {
            List<EmployeeCategory> employeeCategoryList = null;
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
                        cmd.CommandText = "[Accounts].[GetEmployeeCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeCategoryList = new List<EmployeeCategory>();
                                while (sdr.Read())
                                {
                                    EmployeeCategory employeeCategory = new EmployeeCategory();
                                    {
                                        employeeCategory.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employeeCategory.Code);
                                        employeeCategory.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employeeCategory.Name);
                                        employeeCategory.commonObj = new Common();
                                        {
                                            employeeCategory.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : employeeCategory.commonObj.CreatedBy);
                                            employeeCategory.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : employeeCategory.commonObj.CreatedDate);
                                            employeeCategory.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.dateformat) : string.Empty);
                                        }
                                    }
                                    employeeCategoryList.Add(employeeCategory);
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

            return employeeCategoryList;
        }

        public object InsertEmployeeCategory(EmployeeCategory employeeCategory)
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
                        cmd.CommandText = "[Accounts].[InsertEmployeeCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = employeeCategory.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = employeeCategory.Name;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = employeeCategory.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = employeeCategory.commonObj.CreatedDate;
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
            return employeeCategory;
        }

        public object UpdateEmployeeCategory(EmployeeCategory employeeCategory)
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
                        cmd.CommandText = "[Accounts].[UpdateEmployeeCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = employeeCategory.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = employeeCategory.Name;

                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = employeeCategory.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = employeeCategory.commonObj.UpdatedDate;
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

        public object DeleteEmployeeCategory(string Code)
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
                        cmd.CommandText = "[Accounts].[DeleteEmployeeCategory]";
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

       
    }
}