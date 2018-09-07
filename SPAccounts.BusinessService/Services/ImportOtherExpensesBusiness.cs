using SPAccounts.DataAccessObject.DTO;
using System.IO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.BusinessService.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class ImportOtherExpensesBusiness : IImportOtherExpensesBusiness
    {

        private IImportOtherExpensesRepository _importOtherExpensesRepository;

        public ImportOtherExpensesBusiness(IImportOtherExpensesRepository importOtherExpensesRepository)
        {
            _importOtherExpensesRepository = importOtherExpensesRepository;
        }

        public List<UploadedFiles> GetAllUploadedFile()
        {
            return _importOtherExpensesRepository.GetAllUploadedFile();
        }

        public UploadedFiles InsertAttachment(UploadedFiles uploadedFile)
        {
            uploadedFile.FileStatus = "Unvalidated";
            return _importOtherExpensesRepository.InsertAttachment(uploadedFile);
        }

        public UploadedFiles ImportDataToDB(UploadedFiles uploadedFile, string filePath)
        {
            UploadedFiles uploadFileObj = new UploadedFiles();
            List<ImportOtherExpenses> removedData = new List<ImportOtherExpenses>();
            string extension = Path.GetExtension(filePath);
            decimal totalAmount = 0;
            try
            {
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        List<ImportOtherExpenses> ExcelDataXls = _importOtherExpensesRepository.GetExcelDataToList(uploadedFile, filePath, 1);
                        
                        foreach (ImportOtherExpenses ExcelData in ExcelDataXls)
                            {
                            totalAmount = totalAmount + ExcelData.Amount;
                        }
                        removedData = _importOtherExpensesRepository.InsertValidateExpenseData(ExcelDataXls,true);
                        uploadedFile.FileStatus = "Successfully Imported";
                        uploadFileObj = _importOtherExpensesRepository.UpdateUplodedFileDetail(uploadedFile,ExcelDataXls.Count - removedData.Count);
                        uploadFileObj.RemovedDataCount = removedData.Count;
                        uploadFileObj.ImportExpenseList = (removedData.Count != 0) ? removedData : ExcelDataXls;
                        uploadFileObj.TotalAmount = totalAmount;
                        break;
                    case ".xlsx": //Excel 07 to 12
                        List<ImportOtherExpenses> ExcelDataXlsx = _importOtherExpensesRepository.GetExcelDataToList(uploadedFile, filePath, 2);
                        foreach (ImportOtherExpenses ExcelData in ExcelDataXlsx)
                        {
                            totalAmount = totalAmount + ExcelData.Amount;
                        }
                        removedData = _importOtherExpensesRepository.InsertValidateExpenseData(ExcelDataXlsx,true);
                        uploadedFile.FileStatus = "Successfully Imported";
                        uploadFileObj = _importOtherExpensesRepository.UpdateUplodedFileDetail(uploadedFile,ExcelDataXlsx.Count);
                        uploadFileObj.RemovedDataCount = removedData.Count;
                        uploadFileObj.ImportExpenseList = (removedData.Count != 0) ? removedData : ExcelDataXlsx;
                        uploadFileObj.TotalAmount = totalAmount;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadFileObj;
        }

        public UploadedFiles ValidateImportData(UploadedFiles uploadedFile, string filePath)
        {
            List<ImportOtherExpenses> removedData = new List<ImportOtherExpenses>();
            string extension = Path.GetExtension(filePath);
            try
            {
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        List<ImportOtherExpenses> ExcelDataXls = _importOtherExpensesRepository.GetExcelDataToList(uploadedFile, filePath, 1);
                        uploadedFile.RecordCount = ExcelDataXls.Count;
                        removedData = _importOtherExpensesRepository.InsertValidateExpenseData(ExcelDataXls,false);
                        uploadedFile.RemovedDataCount = removedData.Count;
                        uploadedFile.ImportExpenseList = (removedData.Count != 0) ? removedData : ExcelDataXls;

                        break;
                    case ".xlsx": //Excel 07 to 12
                        List<ImportOtherExpenses> ExcelDataXlsx = _importOtherExpensesRepository.GetExcelDataToList(uploadedFile, filePath, 2);
                        uploadedFile.RecordCount = ExcelDataXlsx.Count;
                        removedData = _importOtherExpensesRepository.InsertValidateExpenseData(ExcelDataXlsx,false);
                        uploadedFile.RemovedDataCount = removedData.Count;
                        uploadedFile.ImportExpenseList = (removedData.Count != 0) ? removedData : ExcelDataXlsx;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadedFile;
        }

    }
}