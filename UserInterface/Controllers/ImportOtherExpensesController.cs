using AutoMapper;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.UserInterface.SecurityFilter;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace UserInterface.Controllers
{
    public class ImportOtherExpensesController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        IImportOtherExpensesBusiness _importOtherExpensesBusiness;

        public ImportOtherExpensesController(IImportOtherExpensesBusiness importOtherExpensesBusiness)
        {
            _importOtherExpensesBusiness = importOtherExpensesBusiness;
        }
        #endregion Constructor_Injection 

        #region Index
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #endregion Index

        //------To get all the upload files details for fetching history------//
        #region GetAllUploadedFile
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "R")]
        public string GetAllUploadedFile()
        {
            try
            {
                List<UploadedFiles> uploadedFileList = _importOtherExpensesBusiness.GetAllUploadedFile();
                return JsonConvert.SerializeObject(new { Result = "OK", Records = uploadedFileList });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllUploadedFile

        #region ValidateUploadFile
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "W")]
        public ActionResult ValidateUploadFile()
        {
            UploadedFilesViewModel uploadedFilesVM = new UploadedFilesViewModel();
            UploadedFiles uploadFilesObj = new UploadedFiles();
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;

            HttpPostedFileBase file = files[0];
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (ValidateFileName(fname) != "success")
                    {
                        return Json(new { Result = "WARNING", Message = "Invalid File, Either filename or filetype mismatch !" });
                    }

                    if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/Uploads/"), fname)) == false)
                    {
                        uploadedFilesVM.FilePath = SetFilePath(fname);
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "File uploaded recently" });
                    }
                    uploadedFilesVM.CommonObj = new CommonViewModel();
                    uploadedFilesVM.CommonObj.CreatedBy     = _appUA.UserName;
                    uploadedFilesVM.CommonObj.CreatedDate   = _appUA.DateTime;
                    uploadedFilesVM.CommonObj.UpdatedBy     = _appUA.UserName;
                    uploadedFilesVM.CommonObj.UpdatedDate   = _appUA.DateTime;
                    uploadedFilesVM.FileType                = "OtherExpenses";

                    List<UploadedFiles> uploadedFileList = _importOtherExpensesBusiness.GetAllUploadedFile();
                    object fileExist = (from i in uploadedFileList where uploadedFilesVM.FilePath.Equals("/Content/Uploads/" + i.FilePath) && i.FileStatus.Equals("Successfully Imported") select i).FirstOrDefault();
                    if (fileExist != null)
                    {
                        return Json(new { Result = "ERROR", Message = "File Already Imported!" });
                    }
                    UploadedFiles uploadedFiles = _importOtherExpensesBusiness.InsertAttachment(Mapper.Map<UploadedFilesViewModel, UploadedFiles>(uploadedFilesVM));

                    fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                    file.SaveAs(fname);
                    uploadFilesObj = _importOtherExpensesBusiness.ValidateImportData(uploadedFiles, fname);
                    System.IO.File.Delete(fname);
                }
                else
                {
                    return Json(new { Result = "WARNING", Message = "No files selected. Select one and upload" });
                }
                int Count = uploadFilesObj.ImportExpenseList != null ? uploadFilesObj.RecordCount - uploadFilesObj.ImportExpenseList.Count : uploadFilesObj.RecordCount;
                return Json(new
                {
                    Result          = "OK",
                    Message         = "File Validated Successfully!",
                    ImportExpenseList = JsonConvert.SerializeObject(uploadFilesObj.ImportExpenseList),
                    TotalCount      = uploadFilesObj.RecordCount,
                    RemovedCount    = uploadFilesObj.RemovedDataCount,
                    filename        = Path.GetFileName(uploadFilesObj.FilePath)
                });

            }
            catch (Exception ex)
            {
                System.IO.File.Delete(Server.MapPath("~/Content/Uploads/" + file.FileName));
                return Json(new { Result = "EXCEPTION", Message = ex.Message });
            }
        }
        #endregion ValidateUploadFile

        #region UploadFile
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "W")]
        public ActionResult UploadFile()
        {
            UploadedFilesViewModel uploadedFilesVM = new UploadedFilesViewModel();
            UploadedFiles uploadedFilesObj = new UploadedFiles();
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;

            HttpPostedFileBase file = files[0];
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                        string fname;
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        if(ValidateFileName(fname)!= "success")
                        {
                            return Json(new { Result = "WARNING", Message = "Invalid Filename!" });
                        }

                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/Uploads/"), fname)) == false)
                        {
                            uploadedFilesVM.FilePath = SetFilePath(fname);
                        }
                        else
                        {
                            return Json(new { Result = "ERROR", Message = "File uploaded recently" });
                        }

                        UploadedFilesViewModel uploadedFiles = Mapper.Map<UploadedFiles, UploadedFilesViewModel>((
                            from i in _importOtherExpensesBusiness.GetAllUploadedFile()
                            where uploadedFilesVM.FilePath.Equals("/Content/Uploads/" + i.FilePath)
                            select i).ToArray()[0]
                        );
                        uploadedFiles.CommonObj = new CommonViewModel();
                        uploadedFiles.CommonObj.CreatedBy   = _appUA.UserName;
                        uploadedFiles.CommonObj.CreatedDate = _appUA.DateTime;
                        uploadedFiles.CommonObj.UpdatedBy   = _appUA.UserName;
                        uploadedFiles.CommonObj.UpdatedDate = _appUA.DateTime;
                        fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                        file.SaveAs(fname);
                        uploadedFilesObj = _importOtherExpensesBusiness.ImportDataToDB(Mapper.Map<UploadedFilesViewModel, UploadedFiles>(uploadedFiles), fname);
        
                }
                else
                {
                    return Json(new { Result = "WARNING", Message = "No files selected." });
                }
                return Json(new {   Result          = "OK",
                                    Message         = "File Uploaded Successfully!",
                                    ImportExpenseList = JsonConvert.SerializeObject(uploadedFilesObj.ImportExpenseList),
                                    TotalCount      = uploadedFilesObj.RecordCount,
                                    RemovedCount    = uploadedFilesObj.RemovedDataCount,
                                    FileName        = Path.GetFileName(uploadedFilesObj.FilePath)
                                });
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "EXCEPTION", Message = ex.Message });
            }
        }
        #endregion UploadFile

        //-------To validate filename is in the approved format (eg: OtherExpense_00.00.0000_E0.xlsx)--------//
        #region ValidateFileName
        string ValidateFileName(string fname)
        {
            var regEx = new Regex(@"^OtherExpense_([0]?[0-9]|[12][0-9]|[3][01])[.]([0]?[1-9]|[1][0-2])[.]([0-9]{4}|[0-9]{2})_E[0-9]+?.xlsx$");
            if (regEx.IsMatch(fname))
            {
                return ("success");
            }
            return ("failed");
        }
        #endregion ValidateFileName

        //-------To remove all .xlsx files older than 12 hrs. and to return as file path under uploads-------//
        #region SetFilePath
        string SetFilePath(string fname)
        {
            string[] allFiles = Directory.GetFiles(Server.MapPath("~/Content/Uploads/"));
            foreach (string aFile in allFiles)
            {
                FileInfo fInfo = new FileInfo(aFile);
                if ((fInfo.CreationTime < DateTime.Now.AddHours(-12)) && fInfo.Extension.Equals(".xlsx"))//Gets only .xlsx files older than 12 hrs. 
                {
                    if (!fInfo.Name.Equals("OtherExpense_00.00.0000_E0.xlsx"))
                        { fInfo.Delete(); }
                }
            }

            return "/Content/Uploads/" + fname;
        }
        #endregion  SetFilePath

        #region DownloadTemplate
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "W")]
        public ActionResult DownloadTemplate()
        {
            string filename = "OtherExpense_00.00.0000_E0.xlsx";
            string filepath = Path.Combine(Server.MapPath("~/Content/Uploads/OtherExpense_00.00.0000_E0.xlsx"));
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";//web content type of .xlsx files
            return File(filepath, contentType, filename);
        }
        #endregion DownloadTemplate

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ImportExpense", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.downloadBtn.Visible = true;
                    ToolboxViewModelObj.downloadBtn.Text = "Template";
                    ToolboxViewModelObj.downloadBtn.Title = "Download";
                    ToolboxViewModelObj.downloadBtn.Event = "DownloadTemplate();";

                    ToolboxViewModelObj.HistoryBtn.Visible = true;
                    ToolboxViewModelObj.HistoryBtn.Text = "History";
                    ToolboxViewModelObj.HistoryBtn.Title = "History";
                    ToolboxViewModelObj.HistoryBtn.Event = "FetchHistory();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion ButtonStyling

    }
}