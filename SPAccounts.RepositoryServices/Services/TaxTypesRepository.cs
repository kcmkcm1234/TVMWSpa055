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
    public class TaxTypesRepository : ITaxTypesRepository
    {
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public TaxTypesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
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
                                    TaxTypes _TaxTypesObj = new TaxTypes();
                                    {
                                        _TaxTypesObj.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : _TaxTypesObj.Code);
                                        _TaxTypesObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _TaxTypesObj.Description);
                                        _TaxTypesObj.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : _TaxTypesObj.Rate);

                                    }
                                    taxTypesList.Add(_TaxTypesObj);
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