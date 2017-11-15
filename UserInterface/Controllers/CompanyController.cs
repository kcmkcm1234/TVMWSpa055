using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class CompanyController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();

        ICompaniesBusiness _companiesBusiness;
        IUserBusiness _userBusiness;


        public CompanyController(ICompaniesBusiness companiesBusiness, IUserBusiness userBusiness)
        {
            _companiesBusiness = companiesBusiness;
            _userBusiness = userBusiness;
        }
        #endregion Constructor_Injection 

        // GET: Company
        public ActionResult Index()
        {
            CompaniesViewModel CVM = new CompaniesViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            CVM.userObj = new UserViewModel();
            CVM.userObj.userList = new List<SelectListItem>();


            List<UserViewModel> userList = Mapper.Map<List<User>, List<UserViewModel>>(_userBusiness.GetAllUsers());


            foreach (UserViewModel U in userList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = U.UserName,
                    Value = U.ID.ToString(),
                    Selected = false
                });
            }

            CVM.userObj.userList = selectListItem;
            return View(CVM);
        }

        #region GetAllCompanies
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public string GetAllCompanies()
        {
            try
            {

                List<CompaniesViewModel> CompanyList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CompanyList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllCompanies

        #region GetCompanyDetailsByCode
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public string GetCompanyDetailsByCode(string Code)
        {
            try
            {

                CompaniesViewModel CompanyList = Mapper.Map<Companies, CompaniesViewModel>(_companiesBusiness.GetCompanyDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = CompanyList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetCompanyDetailsByCode

        #region InsertUpdateCompany
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "W")]
        public string InsertUpdateCompany(CompaniesViewModel _companyObj)
        {
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                _companyObj.commonObj = new CommonViewModel();
                _companyObj.commonObj.CreatedBy = _appUA.UserName;
                _companyObj.commonObj.CreatedDate = _appUA.DateTime;
                _companyObj.commonObj.UpdatedBy = _appUA.UserName;
                _companyObj.commonObj.UpdatedDate = _appUA.DateTime;
              
                CompaniesViewModel companyObj = Mapper.Map<Companies, CompaniesViewModel>(_companiesBusiness.InsertUpdateCompany(Mapper.Map<CompaniesViewModel, Companies>(_companyObj)));
             
                if (_companyObj.Code == ""|| _companyObj.Code==null)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = companyObj, Message = "Insertion successfull" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = companyObj, Message = "Updation successfull" });
                }

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateCompany

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    //----added for export button--------------

                    //ToolboxViewModelObj.PrintBtn.Visible = true;
                    //ToolboxViewModelObj.PrintBtn.Text = "Export";
                    //ToolboxViewModelObj.PrintBtn.Title = "Export";
                    //ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    //---------------------------------------
                    break;
                case "Add":

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.resetbtn.Visible = false;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    ToolboxViewModelObj.deletebtn.Visible = false;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bank";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.addbtn.Visible = false;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

#endregion ButtonStyling
    }
}
    