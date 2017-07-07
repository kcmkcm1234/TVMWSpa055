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
    public class PaymentTermsRepository:IPaymentTermsRepository
    {
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public PaymentTermsRepository(IDatabaseFactory databaseFactory)
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

        public PaymentTerms GetPayTermDetails(string Code)
        {
            PaymentTerms _payTermObj = new PaymentTerms();
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
                        cmd.CommandText = "[Accounts].[GetPayTermDetails]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                        _payTermObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _payTermObj.Code);
                                        _payTermObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _payTermObj.Description);
                                        _payTermObj.NoOfDays = (sdr["NoOfDays"].ToString() != "" ? int.Parse(sdr["NoOfDays"].ToString()) : _payTermObj.NoOfDays);
                                        
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

            return _payTermObj;
        }
    }
}