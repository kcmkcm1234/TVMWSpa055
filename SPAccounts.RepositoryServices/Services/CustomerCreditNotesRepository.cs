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
    public class CustomerCreditNotesRepository : ICustomerCreditNotesRepository
    {
        Settings s = new Settings();
        private IDatabaseFactory _databaseFactory;
        public CustomerCreditNotesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<CustomerCreditNotes> GetAllCustomerCreditNotes()
        {
            List<CustomerCreditNotes> customerCreditNotesList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllCustCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCreditNotesList = new List<CustomerCreditNotes>();
                                while (sdr.Read())
                                {
                                    CustomerCreditNotes _customerCreditNotesObj = new CustomerCreditNotes();
                                    {
                                        _customerCreditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _customerCreditNotesObj.ID);
                                        _customerCreditNotesObj.CustomerID = (sdr["CustomerID"].ToString() != "" ?Guid.Parse(sdr["CustomerID"].ToString()) : _customerCreditNotesObj.CustomerID);
                                        _customerCreditNotesObj.OriginComanyCode = (sdr["OriginComanyCode"].ToString() != "" ? sdr["OriginComanyCode"].ToString() : _customerCreditNotesObj.OriginComanyCode);
                                        _customerCreditNotesObj.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : _customerCreditNotesObj.CreditNoteNo);
                                        _customerCreditNotesObj.CreditNoteDate = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()) : _customerCreditNotesObj.CreditNoteDate);
                                        _customerCreditNotesObj.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _customerCreditNotesObj.CreditAmount);
                                        _customerCreditNotesObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _customerCreditNotesObj.Type);
                                        _customerCreditNotesObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerCreditNotesObj.GeneralNotes);
                                     
                                    }
                                    customerCreditNotesList.Add(_customerCreditNotesObj);
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

            return customerCreditNotesList;
        }

    }
}