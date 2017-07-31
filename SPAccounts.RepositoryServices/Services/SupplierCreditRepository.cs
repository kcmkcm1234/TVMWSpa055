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

        #region GetAllSupplierCreditNotes

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

        #endregion GetAllSupplierCreditNotes

        #region InsertSupplierCreditNotes
        public SupplierCreditNote InsertSupplierCreditNotes(SupplierCreditNote _supplierCreditNoteObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[InsertSupplierCreditNote]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CreditToComanyCode", SqlDbType.VarChar, 10).Value = _supplierCreditNoteObj.CreditToComanyCode;
                        cmd.Parameters.Add("@CRNRefNo", SqlDbType.VarChar, 20).Value = _supplierCreditNoteObj.CRNRefNo;
                        cmd.Parameters.Add("@CRNDate", SqlDbType.DateTime).Value = _supplierCreditNoteObj.CRNDateFormatted;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _supplierCreditNoteObj.Amount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 5).Value = _supplierCreditNoteObj.Type;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierCreditNoteObj.GeneralNotes;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierCreditNoteObj.SupplierID;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
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
                        _supplierCreditNoteObj.ID = Guid.Parse(outputID.Value.ToString());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _supplierCreditNoteObj;
        }
        #endregion InsertSupplierCreditNotes

        #region UpdateSupplierCreditNotes
        public object UpdateSupplierCreditNotes(SupplierCreditNote _supplierCreditNoteObj, AppUA ua)
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
                        cmd.CommandText = "[Accounts].[UpdateSupplierCreditNote]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = _supplierCreditNoteObj.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = _supplierCreditNoteObj.SupplierID;
                        cmd.Parameters.Add("@CreditToComanyCode", SqlDbType.VarChar, 10).Value = _supplierCreditNoteObj.CreditToComanyCode;
                        cmd.Parameters.Add("@CRNRefNo", SqlDbType.VarChar, 20).Value = _supplierCreditNoteObj.CRNRefNo;
                        cmd.Parameters.Add("@CRNDate", SqlDbType.DateTime).Value = _supplierCreditNoteObj.CRNDateFormatted;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _supplierCreditNoteObj.Amount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 5).Value = _supplierCreditNoteObj.Type;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = _supplierCreditNoteObj.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = "Anija";
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
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
        #endregion UpdateSupplierCreditNotes

        #region GetSupplierCreditNoteDetails
        public SupplierCreditNote GetSupplierCreditNoteDetails(Guid ID)
        {
            SupplierCreditNote _supplierCreditNote = new SupplierCreditNote();
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
                        cmd.CommandText = "[Accounts].[GetSupplierCreditNoteDetails]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
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
                                    _supplierCreditNote.adjustedAmount = (sdr["AdjAmount"].ToString() != "" ? decimal.Parse(sdr["AdjAmount"].ToString()) : _supplierCreditNote.adjustedAmount);
                                    _supplierCreditNote.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _supplierCreditNote.GeneralNotes);
                                    _supplierCreditNote.SupplierAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : _supplierCreditNote.SupplierAddress);
                                    _supplierCreditNote.commonObj = new Common();
                                    _supplierCreditNote.commonObj.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : _supplierCreditNote.commonObj.CreatedBy);
                                    _supplierCreditNote.commonObj.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.dateformat) : _supplierCreditNote.commonObj.CreatedDateString);
                                    _supplierCreditNote.commonObj.CreatedDate = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()) : _supplierCreditNote.commonObj.CreatedDate);
                                    _supplierCreditNote.commonObj.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : _supplierCreditNote.commonObj.UpdatedBy);
                                    _supplierCreditNote.commonObj.UpdatedDate = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()) : _supplierCreditNote.commonObj.UpdatedDate);
                                    _supplierCreditNote.commonObj.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(settings.dateformat) : _supplierCreditNote.commonObj.UpdatedDateString);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return _supplierCreditNote;
        }
        #endregion GetSupplierCreditNoteDetails

        #region DeleteSupplierCreditNote
        public object DeleteSupplierCreditNote(Guid ID)
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
                        cmd.CommandText = "[Accounts].[DeleteSupplierCreditNote]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = "Anija";
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
        #endregion DeleteSupplierCreditNote

    }
}