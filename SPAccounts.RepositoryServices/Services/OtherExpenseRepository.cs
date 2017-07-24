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
    public class OtherExpenseRepository: IOtherExpenseRepository
    {
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public OtherExpenseRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }


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
                                        _otherExpense.AccountCode = (sdr["AccountCode"].ToString() != "" ? Guid.Parse(sdr["AccountCode"].ToString()) : _otherExpense.AccountCode);
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
                                        _otherExpense.depositAndWithdrwal = new DepositAndWithdrwal()
                                        {
                                            ID = (sdr["DepWithID"].ToString() != "" ? Guid.Parse(sdr["DepWithID"].ToString()) : Guid.Empty)
                                        };
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
    }
}