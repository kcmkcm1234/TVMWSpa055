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
    }
}