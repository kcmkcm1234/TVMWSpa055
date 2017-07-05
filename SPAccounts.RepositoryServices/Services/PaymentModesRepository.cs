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
    public class PaymentModesRepository: IPaymentModesRepository
    {

        private IDatabaseFactory _databaseFactory;
        public PaymentModesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<PaymentModes> GetAllPaymentModes()
        {
            List<PaymentModes> PaymentModesList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllPaymentModes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PaymentModesList = new List<PaymentModes>();
                                while (sdr.Read())
                                {
                                    PaymentModes _pmObj = new PaymentModes();
                                    {
                                        _pmObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : _pmObj.Code);
                                        _pmObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _pmObj.Description);
                                    }
                                    PaymentModesList.Add(_pmObj);
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

            return PaymentModesList;
        }


    }
}