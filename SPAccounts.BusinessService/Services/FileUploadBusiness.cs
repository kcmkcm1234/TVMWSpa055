using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class FileUploadBusiness:IFileUploadBusiness
    {
        IFileUploadRepository _fileRepository;
        public FileUploadBusiness(IFileUploadRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public FileUpload InsertAttachment(FileUpload fileUploadObj)
        {
            return _fileRepository.InsertAttachment(fileUploadObj);
        }
    }
}