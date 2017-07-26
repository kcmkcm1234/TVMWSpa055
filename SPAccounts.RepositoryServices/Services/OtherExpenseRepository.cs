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
        public object DeleteOtherExpense(OtherExpense otherExpense)
        {
            throw new NotImplementedException();
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
                                        _otherExpense.ExpenseDate = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()).ToString(settings.dateformat) : _otherExpense.ExpenseDate);
                                        _otherExpense.AccountCode = (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : _otherExpense.AccountCode);
                                        _otherExpense.PaidFromCompanyCode = (sdr["PaidFromComanyCode"].ToString() != "" ? (sdr["PaidFromComanyCode"].ToString()) : _otherExpense.PaidFromCompanyCode);
                                        _otherExpense.companies = new Companies()
                                        {
                                            Code = (sdr["PaidFromComanyCode"].ToString() != "" ? (sdr["PaidFromComanyCode"].ToString()) : string.Empty),
                                            Name = (sdr["EmpName"].ToString() != "" ? sdr["EmpName"].ToString() : string.Empty)
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
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value = otherExpense.ExpenseRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherExpense.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherExpense.Amount;
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
                        cmd.CommandText = "";
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
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value = otherExpense.ExpenseRef;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherExpense.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherExpense.Amount;
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
        #endregion UpdateOtherExpense
    }
}