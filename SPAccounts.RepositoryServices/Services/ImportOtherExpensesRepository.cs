using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class ImportOtherExpensesRepository : IImportOtherExpensesRepository
    {
        #region Constructor Injection
        AppConst Cobj = new AppConst();
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        private ICompaniesRepository _companiesRepository;
        private IEmployeeRepository _employeeRepository;
        private IChartOfAccountsRepository _chartOfAccountsRepository;
        public ImportOtherExpensesRepository(IDatabaseFactory databaseFactory, ICompaniesRepository companiesRepository, IEmployeeRepository employeeRepository, IChartOfAccountsRepository chartOfAccountsRepository)
        {
            _databaseFactory = databaseFactory;
            _companiesRepository = companiesRepository;
            _employeeRepository = employeeRepository;
            _chartOfAccountsRepository=chartOfAccountsRepository;
        }
        #endregion Constructor Injection
        
        #region InsertAttachment
        public UploadedFiles InsertAttachment(UploadedFiles uploadedFile)
        {
            try
            {
                SqlParameter outputID       = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        cmd.Connection  = con;
                        cmd.CommandText = "[Accounts].[InsertUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 250).Value  = uploadedFile.FilePath;
                        cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 50).Value   = uploadedFile.FileType;
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Value         = uploadedFile.RecordCount;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = uploadedFile.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value    = uploadedFile.CommonObj.CreatedDate;
                        cmd.Parameters.Add("@FileStatus", SqlDbType.NVarChar, 50).Value = uploadedFile.FileStatus;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    uploadedFile.ID = Guid.Parse(outputID.Value.ToString());
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return uploadedFile;
        }
        #endregion InsertAttachment

        #region GetAllUploadedFile
        public List<UploadedFiles> GetAllUploadedFile()
        {
            List<UploadedFiles> uploadedFileList = new List<UploadedFiles>();
            try
            {
                Settings setting  = new Settings();
                string excessPath = "/Content/Uploads/";
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection  = con;
                        cmd.CommandText = "[Accounts].[GetAllUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    UploadedFiles uploadedFile = new UploadedFiles();
                                    {
                                        uploadedFile.ID            = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : uploadedFile.ID);
                                        uploadedFile.FileType      = (sdr["FileType"].ToString() != "" ? (sdr["FileType"].ToString()) : uploadedFile.FileType);
                                        uploadedFile.FilePath      = (sdr["FilePath"].ToString() != "" ? sdr["FilePath"].ToString().Replace(excessPath, "") : uploadedFile.FilePath);
                                        uploadedFile.RecordCount   = (sdr["RecordCount"].ToString() != "" ? int.Parse(sdr["RecordCount"].ToString()) : uploadedFile.RecordCount);
                                        uploadedFile.FileStatus    = (sdr["FileStatus"].ToString() != "" ? sdr["FileStatus"].ToString() : uploadedFile.FileStatus);
                                        uploadedFile.CreatedDate   = (sdr["CreatedDate"]).ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(setting.dateformat) : sdr["CreatedDate"].ToString();
                                    }
                                    uploadedFileList.Add(uploadedFile);
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
            return uploadedFileList;
        }
        #endregion GetAllUploadedFile

        #region GetExcelDataToTable
        public List<ImportOtherExpenses> GetExcelDataToList(UploadedFiles fileUploadObj, string fname, int flag)
        {
            List<ImportOtherExpenses> importOtherExpenseList = new List<ImportOtherExpenses>();
            DataTable ExcelData = null;
            try
            {
                //Insert all values from excel to datatable
                using (OleDbConnection excel_con = _databaseFactory.GetOleDBConnection(flag, fname))
                {
                    excel_con.Open();
                    string sheet1           = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    ExcelData             = new DataTable();
                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    OleDbCommand cmdExcel   = new OleDbCommand();
                    cmdExcel.Connection     = excel_con;
                    cmdExcel.CommandText    = "SELECT * From [" + sheet1 + "]";
                    OleDbDataAdapter oda    = new OleDbDataAdapter();
                    oda.SelectCommand       = cmdExcel;
                    oda.Fill(ExcelData);
                    excel_con.Close();
                }
                //To remove all Empty Row from DataTable
                ExcelData = ExcelData
                    .Rows.Cast<DataRow>()
                    .Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string)))
                    .CopyToDataTable();

                for (int i = 0; i < ExcelData.Rows.Count; i++)
                {
                    DataRow row = ExcelData.Rows[i];
                    ImportOtherExpenses importOtherExpenseObj = new ImportOtherExpenses();

                    importOtherExpenseObj.ExpenseDate   = row["Date"].ToString().Trim();
                    importOtherExpenseObj.AccountCode   = row["AccountCode"].ToString().Trim();
                    importOtherExpenseObj.Company       = row["ExpCompany"].ToString().Trim();
                    importOtherExpenseObj.EmpCode       = row["EmpCode"].ToString().Trim();
                    importOtherExpenseObj.EmpName       = row["EmpName"].ToString().Trim();
                    importOtherExpenseObj.EmpCompany    = row["EmpCompany"].ToString().Trim();
                    importOtherExpenseObj.PaymentMode   = row["PayMode"].ToString().Trim();
                    importOtherExpenseObj.ExpenseRef    = row["PayReference"].ToString().Trim();
                    importOtherExpenseObj.Description   = row["Description"].ToString().Trim();
                    importOtherExpenseObj.Amount        = Decimal.Parse(row["Amount"].ToString().Trim());
                    importOtherExpenseObj.CommonObj     = fileUploadObj.CommonObj;

                    importOtherExpenseList.Add(importOtherExpenseObj);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Column 'Date' does not belong to table ."))
                {
                    throw new Exception("Invalid Excel File Uploaded");
                }
                throw ex;
            }
            return importOtherExpenseList;
        }
        #endregion GetExcelDataToTable
        
        #region InsertValidateRows
        public List<ImportOtherExpenses> InsertValidateExpenseData(List<ImportOtherExpenses> importOtherExpenseList,bool flag)
        {
            List<ChartOfAccounts>   chartOfAccountsList   = _chartOfAccountsRepository.GetAllChartOfAccounts("OE");
            List<Companies>         companiesList         = _companiesRepository.GetAllCompanies();
            List<Employee>          employeeList          = _employeeRepository.GetAllEmployees(null);
            try
            {
                List<ImportOtherExpenses> removedDataList = new List<ImportOtherExpenses>();
                string[] AccountList    = (from i in chartOfAccountsList where i.Type == "OE" select i.Code).ToArray();
                string[] CompanyNames   = (from i in companiesList select i.Name).ToArray();
                string[] EmployeeCodes  = (from i in employeeList select i.Code).ToArray();
                string[] EmployeeNames  = (from i in employeeList select i.Name).ToArray();
                int count=2;
                foreach (ImportOtherExpenses anImportOtherExpense in importOtherExpenseList)
                {
                    if (AccountList.Contains(anImportOtherExpense.AccountCode))
                    {
                        if ( CompanyNames.Contains(anImportOtherExpense.Company) )
                        {
                            if ( (anImportOtherExpense.EmpName.Equals("-") || anImportOtherExpense.EmpName.Equals("") || EmployeeNames.Contains(anImportOtherExpense.EmpName)) && (anImportOtherExpense.EmpCode.Equals("-") || anImportOtherExpense.EmpCode.Equals("") || EmployeeCodes.Contains(anImportOtherExpense.EmpCode)))
                            {
                                ImportOtherExpenses importExpenseReturn = ModifyRow(anImportOtherExpense, companiesList, employeeList);
                                if (flag == true)
                                {
                                    InsertInToOtherExpenses(importExpenseReturn);
                                }
                            }
                            else
                            {
                                anImportOtherExpense.ErrorRow   = count;
                                anImportOtherExpense.Error      = "Invalid Employee";
                                anImportOtherExpense.ExpenseDate = DateTime.Parse(anImportOtherExpense.ExpenseDate).ToString(settings.dateformat);
                                removedDataList.Add(anImportOtherExpense);
                            }
                        }
                        else
                        {
                            anImportOtherExpense.ErrorRow   = count;
                            anImportOtherExpense.Error      = "Invalid Company";
                            anImportOtherExpense.ExpenseDate = DateTime.Parse(anImportOtherExpense.ExpenseDate).ToString(settings.dateformat);
                            removedDataList.Add(anImportOtherExpense);
                        }
                    }
                    else
                    {
                        anImportOtherExpense.ErrorRow   = count;
                        anImportOtherExpense.Error      = "Invalid Account Code";
                        anImportOtherExpense.ExpenseDate = DateTime.Parse(anImportOtherExpense.ExpenseDate).ToString(settings.dateformat);
                        removedDataList.Add(anImportOtherExpense);
                    }
                    count++;
                }

                return removedDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion ValidateRows

        #region ModifyRow
        ImportOtherExpenses ModifyRow(ImportOtherExpenses importOtherExpense, List<Companies> companiesList, List<Employee> employeeList)
        {

            try
            {
                importOtherExpense.PaidFromCompanyCode = (from aCompany in companiesList where aCompany.Name == importOtherExpense.Company select aCompany.Code).ToArray()[0].ToString();
                if (!importOtherExpense.EmpName.Equals("-") || !importOtherExpense.EmpCode.Equals("-"))
                {
                    if(!importOtherExpense.EmpName.Equals("") || !importOtherExpense.EmpCode.Equals(""))
                    {
                        importOtherExpense.EmpID = Guid.Parse((from anEmployee in employeeList where anEmployee.Name == importOtherExpense.EmpName && anEmployee.Code == importOtherExpense.EmpCode select anEmployee.ID).First().ToString());

                    }

                }
                return importOtherExpense;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion ModifyRow

        #region InsertInToOtherExpenses
        void InsertInToOtherExpenses(ImportOtherExpenses importOtherExpense)
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
                        cmd.Connection  = con;
                        cmd.CommandText = "[Accounts].[InsertOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value    = importOtherExpense.ExpenseDate != "-" ? importOtherExpense.ExpenseDate : null;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = importOtherExpense.AccountCode != "-" ? importOtherExpense.AccountCode : null;
                        cmd.Parameters.Add("@PaidFromComanyCode", SqlDbType.VarChar, 10).Value = importOtherExpense.PaidFromCompanyCode != "-" ? importOtherExpense.PaidFromCompanyCode : null;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value  = importOtherExpense.EmpID != Guid.Empty ? importOtherExpense.EmpID : Guid.Empty;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = importOtherExpense.PaymentMode != "-" ? importOtherExpense.PaymentMode : null;
                        if (importOtherExpense.DepWithdID != Guid.Empty)
                        {
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = importOtherExpense.DepWithdID;
                        }
                        cmd.Parameters.AddWithValue("@BankCode", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Refbank", DBNull.Value);
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value  = importOtherExpense.ExpenseRef != "-" ? importOtherExpense.ExpenseRef : null;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = importOtherExpense.Description != "-" ? importOtherExpense.Description : null;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value          = importOtherExpense.Amount;
                        cmd.Parameters.AddWithValue("@IsReverse", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ChequeDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ChequeClearDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ReversalRef", DBNull.Value);
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = importOtherExpense.CommonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value    = importOtherExpense.CommonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        switch (outputStatus.Value.ToString())
                        {
                            case "0":
                                AppConst Cobj           = new AppConst();
                                importOtherExpense.ID   = Guid.Empty;
                                throw new Exception(Cobj.InsertFailure);
                            case "1":
                                importOtherExpense.ID   = Guid.Parse(outputID.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion InsertIntoOtherExpenses

        #region UpdateUplodedFileDetail
        public UploadedFiles UpdateUplodedFileDetail(UploadedFiles uploadedFile,int RowCount)
        {
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

                        cmd.Connection  = con;
                        cmd.CommandText = "[Accounts].[UpdateUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value     = uploadedFile.ID;
                        cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 250).Value  = uploadedFile.FilePath;
                        cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 50).Value   = uploadedFile.FileType;
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Value         = uploadedFile.RecordCount = RowCount;
                        cmd.Parameters.Add("@FileStatus", SqlDbType.NVarChar, 50).Value = uploadedFile.FileStatus;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = uploadedFile.CommonObj.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value    = uploadedFile.CommonObj.UpdatedDate;
                        cmd.ExecuteNonQuery();
                    }
                }
                return uploadedFile;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion UpdateUplodedFileDetail

    }
}