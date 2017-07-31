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
    public class SupplierRepository : ISupplierRepository
    {
        AppConst Cobj = new AppConst();
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SupplierRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllSuppliers
        public List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliersList = null;
            Settings settings = new Settings();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[GetAllSuppliers]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if((sdr!=null)&&(sdr.HasRows))
                            {
                                suppliersList = new List<Supplier>();
                                while(sdr.Read())
                                {
                                    Supplier _SupplierObj = new Supplier();
                                    {
                                        _SupplierObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _SupplierObj.ID);
                                        _SupplierObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _SupplierObj.CompanyName);
                                        _SupplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _SupplierObj.ContactPerson);
                                        _SupplierObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _SupplierObj.ContactEmail);
                                        _SupplierObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _SupplierObj.ContactTitle);
                                        _SupplierObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _SupplierObj.Website);
                                        _SupplierObj.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : _SupplierObj.LandLine);
                                        _SupplierObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _SupplierObj.Mobile);
                                        _SupplierObj.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : _SupplierObj.Fax);
                                        _SupplierObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _SupplierObj.OtherPhoneNos);
                                        _SupplierObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _SupplierObj.BillingAddress);
                                        _SupplierObj.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : _SupplierObj.ShippingAddress);
                                        _SupplierObj.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : _SupplierObj.PaymentTermCode);
                                        _SupplierObj.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : _SupplierObj.TaxRegNo);
                                        _SupplierObj.PANNO = (sdr["PANNo"].ToString() != "" ? sdr["PANNo"].ToString() : _SupplierObj.PANNO);
                                        _SupplierObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _SupplierObj.GeneralNotes);
                                    }
                                    suppliersList.Add(_SupplierObj);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return suppliersList;
        }
        #endregion GetAllSuppliers

        #region GetAllSuppliersForMobile
        public List<Supplier> GetAllSuppliersForMobile()
        {
            List<Supplier> suppliersList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllSuppliersForMobile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                suppliersList = new List<Supplier>();
                                while (sdr.Read())
                                {
                                    Supplier _SupplierObj = new Supplier();
                                    {
                                        _SupplierObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _SupplierObj.CompanyName);
                                        _SupplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _SupplierObj.ContactPerson);
                                        _SupplierObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _SupplierObj.Mobile);
                                        _SupplierObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _SupplierObj.BillingAddress);
                                        _SupplierObj.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : _SupplierObj.OutStanding);
                                    }

                                    suppliersList.Add(_SupplierObj);
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
            return suppliersList;
        }
        #endregion GetAllSuppliersForMobile

        #region GetSupplierForMobileById
        public Supplier GetSupplierDetailsForMobile(Guid ID)
        {
            Supplier _supplierObj = new Supplier();
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
                        cmd.CommandText = "[Accounts].[GetSupplierDetailsById]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _supplierObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _supplierObj.CompanyName);
                                    _supplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _supplierObj.ContactPerson);
                                    _supplierObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _supplierObj.ContactEmail);
                                    _supplierObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _supplierObj.Website);
                                    _supplierObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _supplierObj.Mobile);
                                    _supplierObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _supplierObj.OtherPhoneNos);
                                    _supplierObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _supplierObj.BillingAddress);
                                    _supplierObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _supplierObj.GeneralNotes);
                                    _supplierObj.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : _supplierObj.OutStanding);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _supplierObj;
        }
        #endregion GetSupplierForMobileById

        #region GetSupplierDetails
        public Supplier GetSupplierDetails(Guid ID)
        {
            Supplier _supplierObj = new Supplier();
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
                        cmd.CommandText = "[Accounts].[GetSupplierDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    _supplierObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _supplierObj.ID);
                                    _supplierObj.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : _supplierObj.CompanyName);
                                    _supplierObj.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : _supplierObj.ContactPerson);
                                    _supplierObj.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : _supplierObj.ContactEmail);
                                    _supplierObj.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : _supplierObj.ContactTitle);
                                    _supplierObj.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : _supplierObj.Website);
                                    _supplierObj.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : _supplierObj.LandLine);
                                    _supplierObj.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : _supplierObj.Mobile);
                                    _supplierObj.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : _supplierObj.Fax);
                                    _supplierObj.OtherPhoneNos = (sdr["OtherPhoneNos"].ToString() != "" ? sdr["OtherPhoneNos"].ToString() : _supplierObj.OtherPhoneNos);
                                    _supplierObj.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _supplierObj.BillingAddress);                                   
                                    _supplierObj.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : _supplierObj.PaymentTermCode);
                                    _supplierObj.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : _supplierObj.TaxRegNo);
                                    _supplierObj.PANNO = (sdr["PANNO"].ToString() != "" ? sdr["PANNO"].ToString() : _supplierObj.PANNO);
                                    _supplierObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _supplierObj.GeneralNotes);
                                    _supplierObj.commonObj = new Common();
                                    _supplierObj.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _supplierObj.commonObj.CreatedBy);
                                    _supplierObj.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(s.dateformat) : _supplierObj.commonObj.CreatedDateString);
                                    _supplierObj.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _supplierObj.commonObj.CreatedDate);
                                    _supplierObj.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : _supplierObj.commonObj.UpdatedBy);
                                    _supplierObj.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : _supplierObj.commonObj.UpdatedDate);
                                    _supplierObj.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(s.dateformat) : _supplierObj.commonObj.UpdatedDateString);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _supplierObj;
        }
        #endregion GetSupplierDetails

        #region InsertSupplier
        public Supplier InsertSupplier(Supplier _supplierObj)
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
                        cmd.CommandText = "[Accounts].[InsertSupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = _supplierObj.CompanyName;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = _supplierObj.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = _supplierObj.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = _supplierObj.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = _supplierObj.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = _supplierObj.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = _supplierObj.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = _supplierObj.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = _supplierObj.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierObj.BillingAddress;                      
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = _supplierObj.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = _supplierObj.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = _supplierObj.PANNO;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierObj.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = _supplierObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = _supplierObj.commonObj.CreatedDate;
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
                        _supplierObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierObj;
        }
        #endregion InsertSupplier

        #region UpdateSupplier
        public object UpdateSupplier(Supplier _supplierObj)
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
                        cmd.CommandText = "[Accounts].[UpdateSupplier]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierObj.ID;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = _supplierObj.CompanyName;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = _supplierObj.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = _supplierObj.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = _supplierObj.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = _supplierObj.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = _supplierObj.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = _supplierObj.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = _supplierObj.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = _supplierObj.OtherPhoneNos;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = _supplierObj.BillingAddress;                        
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = _supplierObj.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = _supplierObj.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = _supplierObj.PANNO;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = _supplierObj.commonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = _supplierObj.commonObj.UpdatedDate;
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
        #endregion UpdateSupplier

        #region DeleteSupplier
        public object DeleteSupplier(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteSupplier]";
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
        #endregion DeleteSupplier



       
    }
}