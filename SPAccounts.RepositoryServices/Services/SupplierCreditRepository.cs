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
    public class SupplierCreditRepository : ISupplierCreditRepository
    {
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public SupplierCreditRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SupplierCreditNote> GetAllSupplierCreditNotes()
        {
            List<SupplierCreditNote> supplierCreditNotelist = null;
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
                        cmd.CommandText = "[Accounts].[GetAllSupplierCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierCreditNotelist = new List<SupplierCreditNote>();
                                while (sdr.Read())
                                {
                                    SupplierCreditNote _supplierCreditNote = new SupplierCreditNote();
                                    {
                                        _supplierCreditNote.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _supplierCreditNote.ID);
                                        _supplierCreditNote.supplier = new Supplier();
                                        _supplierCreditNote.supplier.ID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : _supplierCreditNote.supplier.ID);
                                        _supplierCreditNote.supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? (sdr["CompanyName"].ToString()) : _supplierCreditNote.supplier.CompanyName);
                                        _supplierCreditNote.Company = new Companies();
                                        _supplierCreditNote.Company.Code = (sdr["CreditToComanyCode"].ToString() != "" ? (sdr["CreditToComanyCode"].ToString()) : _supplierCreditNote.Company.Code);
                                        _supplierCreditNote.Company.Name = (sdr["CreditToCompanyName"].ToString() != "" ? sdr["CreditToCompanyName"].ToString() : _supplierCreditNote.Company.Name);
                                        _supplierCreditNote.CRNRefNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _supplierCreditNote.CRNRefNo);
                                        _supplierCreditNote.CRNDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(settings.dateformat) : _supplierCreditNote.CRNDate);
                                        _supplierCreditNote.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _supplierCreditNote.Amount);
                                        _supplierCreditNote.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _supplierCreditNote.Type);
                                        _supplierCreditNote.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _supplierCreditNote.GeneralNotes);

                                    }
                                    supplierCreditNotelist.Add(_supplierCreditNote);
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
            return supplierCreditNotelist;
        }



    }
}