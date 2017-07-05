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
    }
}