using SPAccounts.RepositoryServices.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace SPAccounts.RepositoryServices.Services
{
    public class CustomerInvoicesRepository: ICustomerInvoicesRepository
    {

        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CustomerInvoicesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }





        public List<CustomerInvoice> GetAllCustomerInvoices()
        {
            List<CustomerInvoice> CustomerInvoicesList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllCustomerInvoices]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerInvoicesList = new List<CustomerInvoice>();
                                while (sdr.Read())
                                {
                                    CustomerInvoice CIList = new CustomerInvoice();
                                    {
                                        CIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CIList.ID);
                                        CIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : CIList.InvoiceDate);
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.Customer = sdr["Customer"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["InvoiceAmount"].ToString()) : CIList.InvoiceAmount);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : CIList.LastPaymentDate);
                                        CIList.Status = sdr["Status"].ToString();

                                        //------------date formatting-----------------//
                                        CIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : CIList.InvoiceDateFormatted);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);
                                        CIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : CIList.LastPaymentDateFormatted);


                                    }
                                    CustomerInvoicesList.Add(CIList);
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

            return CustomerInvoicesList;
        }

        public CustomerInvoiceSummary GetCustomerInvoicesSummary()
        {
            CustomerInvoiceSummary CustomerInvoiceSummary = null;
          
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
                        cmd.CommandText = "[Accounts].[GetCustomerInvoiceSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerInvoiceSummary = new CustomerInvoiceSummary();
                                if (sdr.Read())
                                {
                                     
                                    {
                                        CustomerInvoiceSummary.OverdueAmount = (sdr["Overdueamt"].ToString() != "" ? Decimal.Parse(sdr["Overdueamt"].ToString()) : CustomerInvoiceSummary.OverdueAmount);
                                        CustomerInvoiceSummary.OverdueInvoices= (sdr["OverdueInvoices"].ToString() != "" ? int.Parse(sdr["OverdueInvoices"].ToString()) : CustomerInvoiceSummary.OverdueInvoices);

                                        CustomerInvoiceSummary.OpenAmount = (sdr["Openamt"].ToString() != "" ? Decimal.Parse(sdr["Openamt"].ToString()) : CustomerInvoiceSummary.OpenAmount);
                                        CustomerInvoiceSummary.OpenInvoices= (sdr["OpenInvoices"].ToString() != "" ? int.Parse(sdr["OpenInvoices"].ToString()) : CustomerInvoiceSummary.OpenInvoices);

                                        CustomerInvoiceSummary.PaidAmount = (sdr["Paidamt"].ToString() != "" ? Decimal.Parse(sdr["Paidamt"].ToString()) : CustomerInvoiceSummary.PaidAmount);
                                        CustomerInvoiceSummary.PaidInvoices = (sdr["PaidInvoices"].ToString() != "" ? int.Parse(sdr["PaidInvoices"].ToString()) : CustomerInvoiceSummary.PaidInvoices);

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

            return CustomerInvoiceSummary;
        }

    }
}