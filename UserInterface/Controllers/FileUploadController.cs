using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    public class FileUploadController : Controller
    {
        IFileUploadBusiness _fileUploadBusiness;
        public FileUploadController(IFileUploadBusiness fileUploadBusiness)
        {
            _fileUploadBusiness = fileUploadBusiness;
        }
        // GET: FileUplad
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                Guid FileID = Guid.NewGuid();
                FileUpload _fileObj = new FileUpload();
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        FileUpload fileuploadObj = new FileUpload();
                        HttpPostedFileBase file = files[i];
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
                        fileuploadObj.AttachmentURL = "/Content/Uploads/" + fname;
                        fileuploadObj.FileSize = file.ContentLength.ToString();
                        fileuploadObj.FileType = file.ContentType;
                        fileuploadObj.FileName = fname;
                        fileuploadObj.ParentID = Request["ParentID"].ToString()!=""?Guid.Parse(Request["ParentID"].ToString()):FileID;
                        fileuploadObj.ParentType = Request["ParentID"].ToString() != "" ? Request["ParentType"]:null;
                        fileuploadObj.commonObj = new Common();
                        fileuploadObj.commonObj.CreatedBy = "Thomson";
                        fileuploadObj.commonObj.CreatedDate = DateTime.Now;
                        fileuploadObj.commonObj.UpdatedBy = "Thomson";
                        fileuploadObj.commonObj.UpdatedDate = DateTime.Now;
                        _fileObj = _fileUploadBusiness.InsertAttachment(fileuploadObj);
                        fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    if(_fileObj.ParentID==FileID)
                    {
                        return Json(new { Result = "OK", Message = "File Uploaded Successfully!", Records = _fileObj });
                    }
                    else
                    {
                       // _fileObj.ParentID = Guid.Empty;
                        return Json(new { Result = "OK", Message = "File Uploaded Successfully!", Records = _fileObj });
                    }
                    
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "Error", Message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { Result = "Error", Message = "No files selected." });
            }
        }
        [HttpGet]
        public string GetAttachments(string ID)
        {
            try
            {

                List<FileUpload> AttachmentList = new List<FileUpload>();
                AttachmentList = _fileUploadBusiness.GetAttachments(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = AttachmentList });
            }
            catch (Exception ex)
            {
                //AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}