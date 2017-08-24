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
                                        CIList.InvoiceType = sdr["InvoiceType"].ToString();

                                        CIList.companiesObj = new Companies();
                                        CIList.companiesObj.Name = sdr["OrginCompany"].ToString();
                                        CIList.customerObj = new Customer();
                                        CIList.customerObj.ID = Guid.Parse(sdr["CustomerID"].ToString());
                                        CIList.customerObj.CompanyName = sdr["CompanyName"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : CIList.TotalInvoiceAmount);
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
        public CustomerInvoice GetCustomerInvoiceDetails(Guid ID)
        {
            CustomerInvoice CIList = null;
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
                        cmd.CommandText = "[Accounts].[GetCustomerInvoiceDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    CIList = new CustomerInvoice();
                                    CIList.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : CIList.ID);
                                    CIList.RefInvoice = (sdr["RefInvoice"].ToString() != "" ? Guid.Parse(sdr["RefInvoice"].ToString()) : CIList.RefInvoice);
                                    CIList.InvoiceType = sdr["InvoiceType"].ToString();

                                    CIList.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : CIList.InvoiceDate);
                                    CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                    CIList.companiesObj = new Companies();
                                    CIList.companiesObj.Code = (sdr["OriginComanyCode"].ToString());
                                    CIList.paymentTermsObj = new PaymentTerms();
                                    CIList.paymentTermsObj.Code = (sdr["PaymentTerm"].ToString());
                                    CIList.customerObj = new Customer();
                                    CIList.customerObj.ID = Guid.Parse(sdr["CustomerID"].ToString());
                                    CIList.customerObj.CompanyName = sdr["CompanyName"].ToString();
                                    CIList.BillingAddress = (sdr["BillingAddress"].ToString());
                                    CIList.GrossAmount = (sdr["GrossAmount"].ToString() != "" ? Decimal.Parse(sdr["GrossAmount"].ToString()) : CIList.GrossAmount);
                                    CIList.Discount = (sdr["Discount"].ToString() != "" ? Decimal.Parse(sdr["Discount"].ToString()) : CIList.Discount);
                                    CIList.TaxAmount = (sdr["TaxAmount"].ToString() != "" ? Decimal.Parse(sdr["TaxAmount"].ToString()) : CIList.TaxAmount);
                                    CIList.TaxPercApplied = (sdr["TaxPercApplied"].ToString() != "" ? Decimal.Parse(sdr["TaxPercApplied"].ToString()) : CIList.TaxPercApplied);
                                    CIList.Notes = (sdr["GeneralNotes"].ToString() != "" ?(sdr["GeneralNotes"].ToString()) : CIList.Notes);
                                    CIList.TaxTypeObj = new TaxTypes();
                                    CIList.TaxTypeObj.Code = sdr["TaxTypeCode"].ToString();
                                    CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                    CIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : CIList.TotalInvoiceAmount);
                                    CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                    CIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : CIList.LastPaymentDate);
                                    
                                    //------------date formatting-----------------//
                                    CIList.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.dateformat) : CIList.InvoiceDateFormatted);
                                    CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);
                                    CIList.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.dateformat) : CIList.LastPaymentDateFormatted);

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

            return CIList;
        }
        //GetCustomerInvoicesSummaryForSA

        public CustomerInvoiceSummary GetCustomerInvoicesSummaryForSA()
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
                        cmd.CommandText = "[Accounts].[GetCustomerInvoiceSummaryForSA]";
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
                                        CustomerInvoiceSummary.OverdueInvoices = (sdr["OverdueInvoices"].ToString() != "" ? int.Parse(sdr["OverdueInvoices"].ToString()) : CustomerInvoiceSummary.OverdueInvoices);

                                        CustomerInvoiceSummary.OpenAmount = (sdr["Openamt"].ToString() != "" ? Decimal.Parse(sdr["Openamt"].ToString()) : CustomerInvoiceSummary.OpenAmount);
                                        CustomerInvoiceSummary.OpenInvoices = (sdr["OpenInvoices"].ToString() != "" ? int.Parse(sdr["OpenInvoices"].ToString()) : CustomerInvoiceSummary.OpenInvoices);

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

        public CustomerInvoice InsertInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[InsertCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OriginCompanyCode", SqlDbType.VarChar, 10).Value =_customerInvoicesObj.companiesObj.Code;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 20).Value = _customerInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = _customerInvoicesObj.customerObj.ID;
                        cmd.Parameters.Add("@RefInvoice", SqlDbType.UniqueIdentifier).Value = _customerInvoicesObj.RefInvoice;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.VarChar, 2).Value = _customerInvoicesObj.InvoiceType;

                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _customerInvoicesObj.paymentTermsObj.Code;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value =_customerInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value =_customerInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar,-1).Value = _customerInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _customerInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _customerInvoicesObj.TaxTypeObj.Code!=""? _customerInvoicesObj.TaxTypeObj.Code:null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxAmount;
                        cmd.Parameters.Add("@TotalInvoiceAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TotalInvoiceAmount;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerInvoicesObj.Notes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = ua.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _customerInvoicesObj.commonObj.CreatedDate;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = _customerInvoicesObj.hdnFileID;
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
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        _customerInvoicesObj.ID =  Guid.Parse(outputID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
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
        public CustomerInvoice UpdateInvoice(CustomerInvoice _customerInvoicesObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[UpdateCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _customerInvoicesObj.ID;
                        cmd.Parameters.Add("@OriginCompanyCode", SqlDbType.VarChar, 10).Value = _customerInvoicesObj.companiesObj.Code;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 20).Value = _customerInvoicesObj.InvoiceNo;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_customerInvoicesObj.hdfCustomerID);
                        cmd.Parameters.Add("@RefInvoice", SqlDbType.UniqueIdentifier).Value = _customerInvoicesObj.RefInvoice;

                        cmd.Parameters.Add("@PaymentTerm", SqlDbType.VarChar, 10).Value = _customerInvoicesObj.paymentTermsObj.Code;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = _customerInvoicesObj.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = _customerInvoicesObj.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _customerInvoicesObj.BillingAddress;
                        cmd.Parameters.Add("@GrossAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.GrossAmount;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = _customerInvoicesObj.Discount;
                        cmd.Parameters.Add("@TaxTypeCode", SqlDbType.VarChar, 10).Value = _customerInvoicesObj.TaxTypeObj.Code != "" ? _customerInvoicesObj.TaxTypeObj.Code : null;
                        cmd.Parameters.Add("@TaxPreApplied", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxPercApplied;
                        cmd.Parameters.Add("@TaxAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TaxAmount;
                        cmd.Parameters.Add("@TotalInvoiceAmount", SqlDbType.Decimal).Value = _customerInvoicesObj.TotalInvoiceAmount;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerInvoicesObj.Notes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _customerInvoicesObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _customerInvoicesObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        //outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        //outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                       // _customerInvoicesObj.ID = new Guid(outputID.Value.ToString());
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
        public List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID)
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
                        cmd.CommandText = "[Accounts].[GetOutStandingInvoices]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = CustID;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
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

                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.TotalInvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : CIList.TotalInvoiceAmount);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.LastPaymentDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : CIList.LastPaymentDate);
                                        CIList.OtherPayments = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : CIList.OtherPayments);
                                        CIList.CustPaymentObj = new CustomerPayments();
                                        CIList.CustPaymentObj.CustPaymentDetailObj = new CustomerPaymentsDetail();
                                        CIList.CustPaymentObj.CustPaymentDetailObj.PaidAmount= (sdr["PaidAmountEdit"].ToString() != "" ? Decimal.Parse(sdr["PaidAmountEdit"].ToString()) : CIList.CustPaymentObj.CustPaymentDetailObj.PaidAmount);
                                        CIList.CustPaymentObj.CustPaymentDetailObj.ID=(sdr["PaymentDetailID"].ToString() != "" ? Guid.Parse(sdr["PaymentDetailID"].ToString()) : CIList.CustPaymentObj.CustPaymentDetailObj.ID);
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

        public CustomerInvoice GetCustomerAdvances(string ID)
        {
            CustomerInvoice CIList = null;
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
                        cmd.CommandText = "[Accounts].[GetCustomerAdvances]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    CIList = new CustomerInvoice();
                                
                                    CIList.customerObj = new Customer();
                                    CIList.customerObj.AdvanceAmount = decimal.Parse(sdr["AdvanceAmount"].ToString());
                                
                                  

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

            return CIList;
        }

        public object DeleteInvoices(Guid ID, string UserName)
        {

            AppConst Cobj = new AppConst();
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
                        cmd.CommandText = "[Accounts].[DeleteInvoices]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(Cobj.InsertFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { Message = Cobj.DeleteSuccess };
        }



        public List<CustomerInvoice> GetOutstandingCustomerInvoices(CustomerInvoice CustomerInvoiceObj)
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
                        cmd.CommandText = "[Accounts].[GetAllOutStandingInvoices]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(CustomerInvoiceObj!=null)
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = CustomerInvoiceObj.customerObj.ID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerInvoicesList = new List<CustomerInvoice>();
                                while (sdr.Read())
                                {
                                    CustomerInvoice CIList = new CustomerInvoice();
                                    {
                                        CIList.ID = (sdr["InvoiceID"].ToString() != "" ? Guid.Parse(sdr["InvoiceID"].ToString()) : CIList.ID);
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.customerObj = new Customer();
                                        CIList.customerObj.ID = Guid.Parse(sdr["CustomerID"].ToString());
                                        CIList.customerObj.ContactPerson = sdr["ContactPerson"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.PaidAmount=(sdr["PaidAmount"].ToString()!=""?Decimal.Parse(sdr["PaidAmount"].ToString()):CIList.PaidAmount);
                                        CIList.DueDays = (sdr["DueDays"].ToString() != "" ? int.Parse(sdr["DueDays"].ToString()) : CIList.DueDays);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);
                                      
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


        public List<CustomerInvoice> GetOpeningCustomerInvoices()
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
                        cmd.CommandText = "[Accounts].[GetAllOpenInvoices]";
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
                                        CIList.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        CIList.customerObj = new Customer();
                                        CIList.customerObj.ID = Guid.Parse(sdr["ID"].ToString());
                                        CIList.customerObj.ContactPerson = sdr["ContactPerson"].ToString();
                                        CIList.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : CIList.PaymentDueDate);
                                        CIList.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : CIList.BalanceDue);
                                        CIList.PaidAmount = (sdr["PaidAmount"].ToString() != "" ? Decimal.Parse(sdr["PaidAmount"].ToString()) : CIList.PaidAmount);
                                        CIList.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.dateformat) : CIList.PaymentDueDateFormatted);

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
    }
}