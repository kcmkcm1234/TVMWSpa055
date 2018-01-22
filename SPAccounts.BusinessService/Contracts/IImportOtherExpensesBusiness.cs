using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.BusinessService.Contracts
{
    public interface IImportOtherExpensesBusiness
    {
        UploadedFiles InsertAttachment(UploadedFiles uploadedFileObj);
        UploadedFiles ImportDataToDB(UploadedFiles uploadedFileObj, string filePath);
        UploadedFiles ValidateImportData(UploadedFiles uploadedFileObj, string filePath);
        List<UploadedFiles> GetAllUploadedFile();
    }
}
