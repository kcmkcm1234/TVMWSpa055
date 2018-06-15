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
    public class CustomerRepository:ICustomerRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public CustomerRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllCustomers
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[Accounts].[GetCustomers]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer _customerObj = new Customer();
                                    {
                                        _customerObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerObj.ID);
                                         _customerObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerObj.CompanyName);
                                        _customerObj.IsInternalComp = (sdr["IsInternalComp"].ToString() != "" ? bool.Parse(sdr["IsInternalComp"].ToString()) : _customerObj.IsInternalComp);
                                        _customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _customerObj.ContactPerson);
                                        _customerObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _customerObj.ContactEmail);
                                        _customerObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _customerObj.ContactTitle);
                                        _customerObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _customerObj.Website);
                                        _customerObj.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : _customerObj.LandLine);
                                        _customerObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _customerObj.Mobile);
                                        _customerObj.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : _customerObj.Fax);
                                        _customerObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _customerObj.OtherPhoneNos);
                                        _customerObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _customerObj.BillingAddress);
                                        _customerObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _customerObj.ShippingAddress);
                                        _customerObj.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : _customerObj.PaymentTermCode);
                                        _customerObj.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : _customerObj.TaxRegNo);
                                        _customerObj.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : _customerObj.PANNO);
                                        _customerObj.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : _customerObj.OutStanding);
                                        _customerObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerObj.GeneralNotes);
                                        _customerObj.commonObj = new Common();
                                        _customerObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _customerObj.commonObj.CreatedBy);
                                        _customerObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : _customerObj.commonObj.CreatedDateString);
                                        _customerObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _customerObj.commonObj.CreatedDate);
                                        _customerObj.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : _customerObj.commonObj.UpdatedBy);
                                        _customerObj.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : _customerObj.commonObj.UpdatedDate);
                                        _customerObj.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(s.dateformat) : _customerObj.commonObj.UpdatedDateString);
                                        //_customerObj.PaymentTermsObj = new PaymentTerms();
                                        _customerObj.PaymentTerm  = (sdr["NoOfDays"].ToString() != "" ? sdr["NoOfDays"].ToString() : _customerObj.PaymentTerm);
                                    }
                                    customerList.Add(_customerObj);
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

            return customerList;
        }
        #endregion GetAllCustomers

        #region GetAllCustomerMobile
        public List<Customer> GetAllCustomersForMobile(Customer cusObj)
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[Accounts].[GetCustomersForMobile]";
                        if (cusObj != null)
                        cmd.Parameters.Add("@includeinternal", SqlDbType.Bit).Value = cusObj.IsInternalComp;
                        cmd.CommandType = CommandType.StoredProcedure;
                    
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer _customerObj = new Customer();
                                    {
                                        _customerObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerObj.ID);
                                        _customerObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _customerObj.ContactTitle);
                                        _customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _customerObj.ContactPerson);
                                        _customerObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _customerObj.Mobile);
                                        _customerObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerObj.CompanyName);
                                        _customerObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _customerObj.BillingAddress);
                                        _customerObj.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : _customerObj.OutStanding);
                                    }
                                    customerList.Add(_customerObj);
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
            
            return customerList;
        }
            
         #endregion GetAllCustomersMobile

        #region GetCustomerDetails
        public Customer GetCustomerDetails(Guid ID)
        {
            Customer _customerObj = new Customer();
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
                        cmd.CommandText = "[Accounts].[GetCustomerDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                        _customerObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerObj.ID);
                                        _customerObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerObj.CompanyName);
                                        _customerObj.IsInternalComp = (sdr["IsInternalComp"].ToString() != "" ? bool.Parse(sdr["IsInternalComp"].ToString()) : _customerObj.IsInternalComp);
                                        _customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _customerObj.ContactPerson);
                                        _customerObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _customerObj.ContactEmail);
                                        _customerObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _customerObj.ContactTitle);
                                        _customerObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _customerObj.Website);
                                        _customerObj.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : _customerObj.LandLine);
                                        _customerObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _customerObj.Mobile);
                                        _customerObj.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : _customerObj.Fax);
                                        _customerObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _customerObj.OtherPhoneNos);
                                        _customerObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _customerObj.BillingAddress);
                                        _customerObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _customerObj.ShippingAddress);
                                        _customerObj.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : _customerObj.PaymentTermCode);
                                        _customerObj.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : _customerObj.TaxRegNo);
                                        _customerObj.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : _customerObj.PANNO);
                                        _customerObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerObj.GeneralNotes);
                                        _customerObj.commonObj = new Common();
                                        _customerObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _customerObj.commonObj.CreatedBy);
                                        _customerObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : _customerObj.commonObj.CreatedDateString);
                                        _customerObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _customerObj.commonObj.CreatedDate);
                                        _customerObj.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : _customerObj.commonObj.UpdatedBy);
                                        _customerObj.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : _customerObj.commonObj.UpdatedDate);
                                        _customerObj.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(s.dateformat) : _customerObj.commonObj.UpdatedDateString);
                                    
                                }
                            }
                        }
                    }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _customerObj;
        }
        #endregion GetCustomerDetails


        #region GetCustomerDetailsForMobile
        public Customer GetCustomerDetailsForMobile(Guid ID)
        {
            Customer _customerObj = new Customer();
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
                        cmd.CommandText = "[Accounts].[GetCustomerDetailsForMobile]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _customerObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _customerObj.CompanyName);
                                    _customerObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _customerObj.ContactTitle);
                                    _customerObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _customerObj.ContactPerson);
                                    _customerObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _customerObj.ContactEmail);
                                    _customerObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _customerObj.Mobile);
                                    _customerObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _customerObj.Website);
                                    _customerObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _customerObj.OtherPhoneNos);
                                    _customerObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _customerObj.BillingAddress);
                                    _customerObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerObj.GeneralNotes);
                                    _customerObj.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : _customerObj.OutStanding);
                                    _customerObj.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : _customerObj.PaymentTermCode);
                                    _customerObj.PaymentTermsObj = new PaymentTerms();
                                    _customerObj.PaymentTermsObj.NoOfDays = (sdr["PaymentTermNoofDays"].ToString() != "" ?int.Parse( sdr["PaymentTermNoofDays"].ToString() ): _customerObj.PaymentTermsObj.NoOfDays);
                                    

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _customerObj;
        }
        #endregion GetCustomerDetailsForMobile

        #region InsertCustomer
        public Customer InsertCustomer(Customer _customerObj)
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
                        cmd.CommandText = "[Accounts].[InsertCustomers]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = _customerObj.CompanyName;
                        cmd.Parameters.Add("@IsInternalComp", SqlDbType.Bit).Value = _customerObj.IsInternalComp;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = _customerObj.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = _customerObj.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = _customerObj.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = _customerObj.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = _customerObj.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = _customerObj.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = _customerObj.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = _customerObj.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _customerObj.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = _customerObj.ShippingAddress;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = _customerObj.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = _customerObj.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = _customerObj.PANNO;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerObj.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _customerObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value =_customerObj.commonObj.CreatedDate;
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
                        _customerObj.ID =Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _customerObj;
        }
        #endregion InsertCustomer

        #region UpdateCustomer
        public object UpdateCustomer(Customer _customerObj)
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
                        cmd.CommandText = "[Accounts].[UpdateCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _customerObj.ID;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = _customerObj.CompanyName;
                        cmd.Parameters.Add("@IsInternalComp", SqlDbType.Bit).Value = _customerObj.IsInternalComp;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = _customerObj.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = _customerObj.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = _customerObj.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = _customerObj.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = _customerObj.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = _customerObj.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = _customerObj.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = _customerObj.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _customerObj.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = _customerObj.ShippingAddress;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = _customerObj.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = _customerObj.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = _customerObj.PANNO;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _customerObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _customerObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _customerObj.commonObj.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":

                        throw new Exception(Cobj.UpdateFailure);
                   
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
                Message = Cobj.UpdateSuccess
            };
        }
        #endregion UpdateCustomer

        #region DeleteCustomer
        public object DeleteCustomer(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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
        #endregion DeleteCustomer

        #region GetAllTitles
        public List<Titles> GetAllTitles()
        {
            List<Titles> titlesList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllTitles]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                titlesList = new List<Titles>();
                                while (sdr.Read())
                                {
                                    Titles _titlesObj = new Titles();
                                    {                                        
                                        _titlesObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : _titlesObj.Title);
                                      
                                    }
                                    titlesList.Add(_titlesObj);
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

            return titlesList;
        }
        #endregion GetAllTitles

    }
}