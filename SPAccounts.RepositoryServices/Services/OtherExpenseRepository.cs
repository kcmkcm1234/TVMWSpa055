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
    public class OtherExpenseRepository : IOtherExpenseRepository
    {
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public OtherExpenseRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region DeleteOtherExpense
        public object DeleteOtherExpense(Guid ID, string UserName)
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
                        cmd.CommandText = "[Accounts].[DeleteOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UserName;
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
        #endregion DeleteOtherExpense

        #region GetAllOtherExpenses
        public List<OtherExpense> GetAllOtherExpenses()
        {
            List<OtherExpense> otherExpenselist = null;
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
                        cmd.CommandText = "[Accounts].[GetAllOtherExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenselist = new List<OtherExpense>();
                                while (sdr.Read())
                                {
                                    OtherExpense _otherExpense = new OtherExpense();
                                    {
                                        _otherExpense.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _otherExpense.ID);
                                        _otherExpense.RefNo = (sdr["RefNo"].ToString() != "" ? sdr["RefNo"].ToString() : string.Empty);
                                        _otherExpense.ReversalRef = (sdr["ReversalRef"].ToString() != "" ? sdr["ReversalRef"].ToString() : string.Empty);
                                        _otherExpense.ExpenseDate = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()).ToString(settings.dateformat) : _otherExpense.ExpenseDate);
                                        _otherExpense.EmpTypeCode = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : string.Empty);
                                        _otherExpense.ExpenseRef = (sdr["ExpenseRef"].ToString() != "" ? sdr["ExpenseRef"].ToString() : string.Empty);
                                        _otherExpense.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? sdr["ReferenceNo"].ToString() : string.Empty);
                                        _otherExpense.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : string.Empty);
                                        _otherExpense.chartOfAccountsObj = new ChartOfAccounts()
                                        {
                                            Code = (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : string.Empty),
                                            TypeDesc = (sdr["AccountTypeDescription"].ToString() != "" ? sdr["AccountTypeDescription"].ToString() : string.Empty),
                                            ISEmploy = (sdr["ISEmpApplicable"].ToString() != "" ? bool.Parse(sdr["ISEmpApplicable"].ToString()) : false),
                                        };
                                        _otherExpense.AccountCode = (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : _otherExpense.AccountCode);
                                        _otherExpense.PaidFromCompanyCode = (sdr["PaidFromComanyCode"].ToString() != "" ? (sdr["PaidFromComanyCode"].ToString()) : _otherExpense.PaidFromCompanyCode);
                                        _otherExpense.companies = new Companies()
                                        {
                                            Code = (sdr["PaidFromComanyCode"].ToString() != "" ? (sdr["PaidFromComanyCode"].ToString()) : string.Empty),
                                            Name = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : string.Empty)

                                        };
                                        _otherExpense.employee = new Employee()
                                        {
                                            ID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : Guid.Empty),
                                            Name = (sdr["EmpName"].ToString() != "" ? sdr["EmpName"].ToString() : string.Empty)
                                        };
                                        _otherExpense.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : _otherExpense.PaymentMode);
                                        _otherExpense.depositAndWithdrwal = new DepositAndWithdrawals()
                                        {
                                            ID = (sdr["DepWithID"].ToString() != "" ? Guid.Parse(sdr["DepWithID"].ToString()) : Guid.Empty)
                                        };
                                        //_otherExpense.depositAndWithdrwal.ID = (sdr["DepWithID"].ToString() != "" ? Guid.Parse(sdr["DepWithID"].ToString()) : Guid.Empty);
                                        _otherExpense.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _otherExpense.BankCode);
                                        _otherExpense.ExpenseRef = (sdr["ExpenseRef"].ToString() != "" ? (sdr["ExpenseRef"].ToString()) : _otherExpense.ExpenseRef);
                                        _otherExpense.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _otherExpense.Description);
                                        _otherExpense.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _otherExpense.Amount);
                                        _otherExpense.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(settings.dateformat) : _otherExpense.ChequeDate);
                                        _otherExpense.commonObj = new Common()
                                        {
                                            CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.dateformat) : string.Empty)
                                        };

                                    }

                                    otherExpenselist.Add(_otherExpense);
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
            return otherExpenselist;
        }
        #endregion GetAllOtherExpenses

        #region InsertOtherExpense
        public OtherExpense InsertOtherExpense(OtherExpense otherExpense)
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
                        cmd.CommandText = "[Accounts].[InsertOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = otherExpense.ExpenseDate;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = otherExpense.AccountCode;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = otherExpense.PaidFromCompanyCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = otherExpense.EmpID;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = otherExpense.PaymentMode;
                        if (otherExpense.DepWithID != Guid.Empty)
                        {
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = otherExpense.DepWithID;
                        }
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = otherExpense.BankCode;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = otherExpense.ReferenceBank;
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value = otherExpense.ExpenseRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherExpense.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherExpense.Amount;
                        cmd.Parameters.Add("@IsReverse", SqlDbType.Bit).Value = otherExpense.IsReverse;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = otherExpense.ChequeDate;
                        cmd.Parameters.Add("@ReversalRef", SqlDbType.VarChar, 20).Value = otherExpense.ReversalRef;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = otherExpense.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = otherExpense.commonObj.CreatedDate;
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
                        otherExpense.ID = Guid.Empty;
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        otherExpense.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return otherExpense;
        }
        #endregion InsertOtherExpense

        #region UpdateOtherExpense
        public OtherExpense UpdateOtherExpense(OtherExpense otherExpense)
        {
            try
            {
                SqlParameter outputStatus = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[UpdateOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = otherExpense.ID;
                        cmd.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = otherExpense.ExpenseDate;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = otherExpense.AccountCode;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = otherExpense.PaidFromCompanyCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = otherExpense.EmpID;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = otherExpense.PaymentMode;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = otherExpense.ReferenceBank;
                        if (otherExpense.DepWithID != Guid.Empty)
                        {
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = otherExpense.DepWithID;
                        }
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = otherExpense.BankCode;
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value = otherExpense.ExpenseRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherExpense.Description;
                        cmd.Parameters.Add("@IsReverse", SqlDbType.Bit).Value = otherExpense.IsReverse;
                        if(otherExpense.IsReverse)
                        cmd.Parameters.Add("@ReversalRef", SqlDbType.VarChar, 20).Value = otherExpense.ReversalRef;

                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherExpense.Amount;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = otherExpense.ChequeDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = otherExpense.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = otherExpense.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return otherExpense;
        }
        #endregion UpdateOtherExpense

        #region summary
        public OtherExpSummary GetOtherExpSummary(int month, int year, string Company)
        {
            OtherExpSummary OES = new OtherExpSummary();
            OES.ItemsList = new List<OtherExpSummaryItem>();
            OtherExpSummaryItem OEI = null;
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
                        cmd.CommandText = "[Accounts].[GetOtherExpensesSummary]";
                        cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                        cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 10).Value = Company;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    OEI = new OtherExpSummaryItem();
                                    OEI.Head = (sdr["AccountTypeDescription"].ToString() != "" ? (sdr["AccountTypeDescription"].ToString()) : OEI.Head);
                                    OEI.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : OEI.Amount);
                                    OEI.color = "red";

                                    OES.ItemsList.Add(OEI);


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

            return OES;
        }
        #endregion summary

        #region GetExpenseDetailsByValue
        public List<OtherExpense> GetExpenseTypeDetails(OtherExpense expObj)
        {
            List<OtherExpense> chartofAccountsList = null;
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
                        cmd.CommandText = "[Accounts].[GetOtherExpenseSummaryForMobile]";
                        cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = expObj.chartOfAccountsObj.startdate;
                        cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = expObj.chartOfAccountsObj.enddate;
                        cmd.Parameters.Add("@days", SqlDbType.Int).Value = expObj.chartOfAccountsObj.days;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartofAccountsList = new List<OtherExpense>();
                                while (sdr.Read())
                                {
                                    OtherExpense _chartofAccountsObj = new OtherExpense();
                                    {
                                        _chartofAccountsObj.chartOfAccountsObj = new ChartOfAccounts();
                                        _chartofAccountsObj.chartOfAccountsObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : _chartofAccountsObj.chartOfAccountsObj.Type);
                                        _chartofAccountsObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _chartofAccountsObj.Amount);


                                        chartofAccountsList.Add(_chartofAccountsObj);
                                    }
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

            return chartofAccountsList;
        }
        #endregion GetExpenseDetailsByValue


        public OtherExpense GetOpeningBalance(string OpeningDate)
        {
            OtherExpense OtherExpenseObj = null;
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
                        cmd.CommandText = "[Accounts].[GetOpeningBalance]";
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = OpeningDate;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OtherExpenseObj = new OtherExpense();
                                while (sdr.Read())
                                {
                                    OtherExpenseObj.OpeningBank = (sdr["BankOpening"].ToString() != "" ? (sdr["BankOpening"].ToString()) : OtherExpenseObj.OpeningBank);
                                    OtherExpenseObj.OpeningNCBank = (sdr["BankNCOpening"].ToString() != "" ? (sdr["BankNCOpening"].ToString()) : OtherExpenseObj.OpeningNCBank);
                                    OtherExpenseObj.OpeningCash = (sdr["CashOpening"].ToString() != "" ? (sdr["CashOpening"].ToString()) : OtherExpenseObj.OpeningCash);
                                    OtherExpenseObj.UndepositedCheque = (sdr["UndepositedChq"].ToString() != "" ? (sdr["UndepositedChq"].ToString()) : OtherExpenseObj.UndepositedCheque);
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




            return OtherExpenseObj;
        }

        #region GetBankWiseBalance
        /// <summary>
        /// To Get Balance on Bank Wise
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public List<OtherExpense> GetBankWiseBalance(string Date)
        {
            List<OtherExpense> otherExpenseList = null;
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
                        cmd.CommandText = "[Accounts].[GetBankWiseBalance]";
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenseList = new List<OtherExpense>();
                                while (sdr.Read())
                                {
                                    OtherExpense _otherExpense = new OtherExpense();
                                    {
                                        _otherExpense.BankCode = (sdr["BankCode"].ToString() != "" ? (sdr["BankCode"].ToString()) : _otherExpense.BankCode);
                                        _otherExpense.BankName = (sdr["BankName"].ToString() != "" ? (sdr["BankName"].ToString()) : _otherExpense.BankName);
                                        _otherExpense.TotalAmount = (sdr["TotalAmount"].ToString() != "" ? (sdr["TotalAmount"].ToString()) : _otherExpense.TotalAmount);
                                    }
                                    otherExpenseList.Add(_otherExpense);
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

            return otherExpenseList;
        }
        #endregion GetBankWiseBalance




        #region ValidateRefno

        public object Validate(OtherExpense _otherexpenseObj)
        {
            AppConst appcust = new AppConst();
            SqlParameter outputStatus = null;
            SqlParameter outputStatus1 = null;
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
                        cmd.CommandText = "[Accounts].[ValidateOtherExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = _otherexpenseObj.ExpenseRef;
                        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = _otherexpenseObj.ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus1 = cmd.Parameters.Add("@message", SqlDbType.VarChar, 100);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputStatus1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

            }


            catch (Exception ex)

            {
                return new { Message = ex.ToString(), Status = -1 };
            }

            return new { Message = outputStatus1.Value.ToString(), Status = outputStatus.Value };

        }

        #endregion ValidateRefno

    }

}

   