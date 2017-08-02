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
    public class DashboardRepository: IDashboardRepository
    {

        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DashboardRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }



        public MonthlyRecap GetMonthlyRecap(string Company)
        {
            MonthlyRecap monthlyRecap = new MonthlyRecap();
            monthlyRecap.MonthlyRecapItemList = new List<MonthlyRecapItem>();
            MonthlyRecapItem MRI = null;
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
                        cmd.CommandText = "[Accounts].[MonthlyRecap]";
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 10).Value = Company;
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    MRI = new MonthlyRecapItem();
                                    MRI.Period = (sdr["Period"].ToString() != "" ? (sdr["Period"].ToString()) : MRI.Period);
                                    MRI.INAmount = (sdr["INAmount"].ToString() != "" ? decimal.Parse(sdr["INAmount"].ToString()) : MRI.INAmount);
                                    MRI.ExAmount = (sdr["ExAmount"].ToString() != "" ? decimal.Parse(sdr["ExAmount"].ToString()) : MRI.ExAmount);

                                    monthlyRecap.MonthlyRecapItemList.Add(MRI);


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

            return monthlyRecap;
        }


        public List<SalesSummary> GetSalesSummaryChart(SalesSummary dueObj)
        {
            List<SalesSummary> SalesSummaryList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[Accounts].[GetSalesSummaryChart]";
                        cmd.Parameters.Add("@duration", SqlDbType.NVarChar, 20).Value = dueObj.duration;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SalesSummaryList = new List<SalesSummary>();
                                while (sdr.Read())
                                {
                                    SalesSummary SSList = new SalesSummary();
                                    {
                                        SSList.Period = (sdr["Period"].ToString() != "" ? sdr["Period"].ToString() : SSList.Period);
                                        SSList.Amount = (sdr["Amount"].ToString() != "" ? sdr["Amount"].ToString() : SSList.Amount);

                                    }
                                    SalesSummaryList.Add(SSList);
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

            return SalesSummaryList;
        }
    }
}