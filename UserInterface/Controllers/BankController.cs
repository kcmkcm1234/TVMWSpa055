using AutoMapper;
using Newtonsoft.Json;
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
    public class BankController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
       
        public BankController(IBankBusiness bankBusiness,ICompaniesBusiness companiesBusiness)
        {
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
        }
        #endregion Constructor_Injection 

        // GET: Bank
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult Index()
        {
            BankViewModel bankViewModel = null;
            try
            {
                bankViewModel = new BankViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Technician Drop down bind
                List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
                companiesList = companiesList == null ? null : companiesList.OrderBy(attset => attset.Name).ToList();
                foreach (CompaniesViewModel cvm in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
                bankViewModel.CompaniesList = selectListItem;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(bankViewModel);
        }

        #region GetAllBanks
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public string GetAllBanks()
        {
            try
            {

                List<BankViewModel> BankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = BankList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllBanks

        #region GetBankDetailsByCode
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public string GetBankDetailsByCode(string Code)
        {
            try
            {

                BankViewModel BankList = Mapper.Map<Bank, BankViewModel>(_bankBusiness.GetBankDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = BankList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetBankDetailsByCode


        #region InsertUpdateBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "W")]
        public string InsertUpdateBank(BankViewModel _bankObj)
        {
            try
            {
                
                    object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _bankObj.commonObj = new CommonViewModel();
                _bankObj.commonObj.CreatedBy = _appUA.UserName;
                _bankObj.commonObj.CreatedDate = _appUA.DateTime;
                _bankObj.commonObj.UpdatedBy = _appUA.UserName;
                _bankObj.commonObj.UpdatedDate = _appUA.DateTime;
                if (!string.IsNullOrEmpty(_bankObj.hdnCode))
                    {
                        _bankObj.Code = _bankObj.hdnCode;
                    }
                    
                    result = _bankBusiness.InsertUpdateBank(Mapper.Map<BankViewModel, Bank>(_bankObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
               
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateBank

        #region DeleteBank
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "D")]
        public string DeleteBank(string code)
        {

            try
            {
                object result = null;
              
              result=  _bankBusiness.DeleteBank(code);
              return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteBank

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
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


                    break;
                case "Edit":
                   
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "openNav();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bank";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bank";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

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
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion


    }
}