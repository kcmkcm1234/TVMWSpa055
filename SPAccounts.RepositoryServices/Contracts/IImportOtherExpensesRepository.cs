using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IImportOtherExpensesRepository
    {
        UploadedFiles InsertAttachment(UploadedFiles uploadedFileObj);
        List<ImportOtherExpenses> GetExcelDataToList(UploadedFiles uploadedFileObj, string fileName, int flag);
        List<ImportOtherExpenses> InsertValidateExpenseData(List<ImportOtherExpenses> importOtherExpenseList,bool flag);
        UploadedFiles UpdateUplodedFileDetail(UploadedFiles uploadedFileObj, int rowCount);
        List<UploadedFiles> GetAllUploadedFile();
    }
}
