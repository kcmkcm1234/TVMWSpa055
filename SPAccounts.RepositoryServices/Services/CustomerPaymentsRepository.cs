using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SPAccounts.RepositoryServices.Services
{
    public class CustomerPaymentsRepository:ICustomerPaymentsRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CustomerPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<CustomerPayments> GetAllCustomerPayments()
        {
            List<CustomerPayments> ExpenseTypelist = null;
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
                        cmd.CommandText = "[Accounts].[GetAllCustomerPayments]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ExpenseTypelist = new List<CustomerPayments>();
                                while (sdr.Read())
                                {
                                    CustomerPayments CustPaymentsObj = new CustomerPayments();

                                    {
                                        CustPaymentsObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CustPaymentsObj.ID);
                                        CustPaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.PaymentDateFormatted);
                                        CustPaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : CustPaymentsObj.PaymentRef);
                                        CustPaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : CustPaymentsObj.EntryNo);
                                        CustPaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : CustPaymentsObj.PaymentMode);
                                        CustPaymentsObj.TotalRecdAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()): CustPaymentsObj.TotalRecdAmt);
                                        CustPaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : CustPaymentsObj.AdvanceAmount);

                                        CustPaymentsObj.customerObj = new Customer();
                                        CustPaymentsObj.customerObj.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : CustPaymentsObj.customerObj.CompanyName);
                                        CustPaymentsObj.customerObj.ID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : CustPaymentsObj.customerObj.ID);
                                        CustPaymentsObj.customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : CustPaymentsObj.customerObj.ContactPerson);

                                    };

                                    ExpenseTypelist.Add(CustPaymentsObj);
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
            return ExpenseTypelist;
        }

        public CustomerPayments GetCustomerPaymentsByID(string ID)
        {
            CustomerPayments CustPaymentsObj = new CustomerPayments();
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[Accounts].[GetCustomerPaymentsDetails]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    CustPaymentsObj.RecdToComanyCode = (sdr["RecdToComanyCode"].ToString() != "" ? sdr["RecdToComanyCode"].ToString() : CustPaymentsObj.RecdToComanyCode);
                                    CustPaymentsObj.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : CustPaymentsObj.PaymentDateFormatted);
                                    CustPaymentsObj.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : CustPaymentsObj.PaymentRef);
                                    CustPaymentsObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : CustPaymentsObj.EntryNo);
                                    CustPaymentsObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : CustPaymentsObj.PaymentMode);
                                    CustPaymentsObj.TotalRecdAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : CustPaymentsObj.TotalRecdAmt);
                                    CustPaymentsObj.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : CustPaymentsObj.AdvanceAmount);
                                    CustPaymentsObj.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : CustPaymentsObj.BankCode);
                                    CustPaymentsObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : CustPaymentsObj.GeneralNotes);
                                    CustPaymentsObj.customerObj = new Customer();
                                    CustPaymentsObj.customerObj.ID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : CustPaymentsObj.customerObj.ID);
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
            return CustPaymentsObj;

        }
    }
}