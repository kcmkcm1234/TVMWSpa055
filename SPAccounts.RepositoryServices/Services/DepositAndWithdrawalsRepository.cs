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
    public class DepositAndWithdrawalsRepository : IDepositAndWithdrawalsRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();

        private IDatabaseFactory _databaseFactory;
        public DepositAndWithdrawalsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        //#region GetAllDepositAndWithdrawals
        //public List<DepositAndWithdrawals> GetAllDepositAndWithdrawals()
        //{
        //    List<DepositAndWithdrawals> depositAndWithdrawalsList = null;
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[Accounts].[GetAllOtherIncome]";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if ((sdr != null) && (sdr.HasRows))
        //                    {
        //                        depositAndWithdrawalsList = new List<DepositAndWithdrawals>();
        //                        while (sdr.Read())
        //                        {
        //                            DepositAndWithdrawals _depositAndWithdrawalsObj = new DepositAndWithdrawals();
        //                            {
        //                                _depositAndWithdrawalsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _depositAndWithdrawalsObj.ID);
        //                                _depositAndWithdrawalsObj.IncomeDate = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()) : _depositAndWithdrawalsObj.IncomeDate);
        //                                _depositAndWithdrawalsObj.AccountCode = (sdr["AccountCode"].ToString() != "" ? (sdr["AccountCode"].ToString()) : _depositAndWithdrawalsObj.AccountCode);
        //                                _depositAndWithdrawalsObj.PaymentRcdComanyCode = (sdr["PaymentRcdComanyCode"].ToString() != "" ? sdr["PaymentRcdComanyCode"].ToString() : _depositAndWithdrawalsObj.PaymentRcdComanyCode);
        //                                _depositAndWithdrawalsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : _depositAndWithdrawalsObj.PaymentMode);
        //                                _depositAndWithdrawalsObj.DepWithdID = (sdr["DepWithdID"].ToString() != "" ? Guid.Parse(sdr["DepWithdID"].ToString()) : _depositAndWithdrawalsObj.DepWithdID);
        //                                _depositAndWithdrawalsObj.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _depositAndWithdrawalsObj.BankCode);
        //                                _depositAndWithdrawalsObj.IncomeRef = (sdr["IncomeRef"].ToString() != "" ? sdr["IncomeRef"].ToString() : _depositAndWithdrawalsObj.IncomeRef);
        //                                _depositAndWithdrawalsObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : _depositAndWithdrawalsObj.Description);
        //                                _depositAndWithdrawalsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _depositAndWithdrawalsObj.Amount);

        //                                _depositAndWithdrawalsObj.IncomeDateFormatted = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()).ToString(s.dateformat) : _depositAndWithdrawalsObj.IncomeDateFormatted);

        //                                _depositAndWithdrawalsObj.slNo = slno;
        //                            }
        //                            depositAndWithdrawalsList.Add(_depositAndWithdrawalsObj);
        //                            slno++;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return depositAndWithdrawalsList;
        //}
        //#endregion GetAllDepositAndWithdrawals

    }
}