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
                                        //CIList.Customer = sdr["Customer"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.GrossAmount = (sdr["GrossAmount"].ToString() != "" ? Decimal.Parse(sdr["GrossAmount"].ToString()) : CIList.GrossAmount);
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
        public CustomerInvoice InsertUpdateInvoice(CustomerInvoice _customerInvoicesObj, UA ua)
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
                        cmd.CommandText = "[InsertCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OriginCompanyCode", SqlDbType.NVarChar, 5).Value =_customerInvoicesObj.OriginCompanyCode;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 20).Value = _customerInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.SmallDateTime).Value = _customerInvoicesObj.customerObj.ID;
                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.NVarChar, 250).Value = _customerInvoicesObj.paymentTermsObj.Code;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.NVarChar, -1).Value =_customerInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.Decimal).Value =_customerInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.Xml).Value = _customerInvoicesObj.customerObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _customerInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.NVarChar, 10).Value = _customerInvoicesObj.taxTypesObj.Code;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxAmount;
                        cmd.Parameters.Add("@TotalInvoiceAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TotalInvoiceAmount;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerInvoicesObj.Notes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = ua.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _customerInvoicesObj.commonObj.CreatedDate;
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
                        Const Cobj = new Const();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _customerInvoicesObj.ID = new Guid(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _customerInvoicesObj;
        }

    }
}