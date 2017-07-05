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
    public class MasterRepository:IMasterRepository
    {
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public MasterRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<PaymentTerms> GetAllPayTerms()
        {
            List<PaymentTerms> payTermList = null;
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
                        cmd.CommandText = "[Accounts].[GetPayTerms]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                payTermList = new List<PaymentTerms>();
                                while (sdr.Read())
                                {
                                    PaymentTerms _payTermObj = new PaymentTerms();
                                    {
                                        _payTermObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _payTermObj.Code);
                                        _payTermObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _payTermObj.Description);
                                        _payTermObj.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : _payTermObj.NoOfDays);
                                        
                                    }
                                    payTermList.Add(_payTermObj);
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

            return payTermList;
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
                                        _companyObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _companyObj.Description);
                                        _companyObj.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : _companyObj.Name);
                                        _companyObj.ShippingAddress= (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _companyObj.ShippingAddress);
                                        _companyObj.BillingAddress= (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _companyObj.BillingAddress);
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
    }
}