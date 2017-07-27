using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class OtherExpensesController : Controller
    {
        // GET: OtherExpenses
        AppConst c = new AppConst();
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        public OtherExpensesController(IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
        }
        public ActionResult Index()
        {
            OtherExpenseViewModel otherExpenseViewModelObj = null;
            try
            {
                otherExpenseViewModelObj = new OtherExpenseViewModel();
               
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<ChartOfAccountsViewModel> chartOfAccountList = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_otherExpenseBusiness.GetAllAccountTypes("OE"));
                foreach (ChartOfAccountsViewModel cav in chartOfAccountList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cav.TypeDesc,
                        Value = cav.Code,
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.AccountTypes = selectListItem;
                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                List<BankViewModel> bankList = Mapper.Map<List<Bank>, List<BankViewModel>>(_otherExpenseBusiness.GetAllBankes());
                foreach (BankViewModel bvm in bankList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = bvm.Name,
                        Value = bvm.Code,
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.bankList = selectListItem;

                otherExpenseViewModelObj.CompanyList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_otherExpenseBusiness.GetAllCompanies());
                foreach (CompaniesViewModel cvm in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = cvm.Name,
                        Value = cvm.Code.ToString(),
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.CompanyList = selectListItem;

                otherExpenseViewModelObj.paymentModeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentModesViewModel> PaymentModeList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_otherExpenseBusiness.GetAllPaymentModes());
                foreach (PaymentModesViewModel PMVM in PaymentModeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PMVM.Description,
                        Value = PMVM.Code,
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.paymentModeList = selectListItem;
                selectListItem = new List<SelectListItem>();
                List<EmployeeViewModel> empList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployees());
                foreach (EmployeeViewModel evm in empList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = evm.Name,
                        Value = evm.ID.ToString(),
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.EmployeeList = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(otherExpenseViewModelObj);
            
        }



        #region GetAllOtherExpenses
        [HttpGet]
        public string GetAllOtherExpenses(string ExpenseDate, string DefaultDate)
        {
            try
            {
                List<OtherExpenseViewModel> otherExpenseViewModelList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetAllOtherExpenses());
                if(!string.IsNullOrEmpty(ExpenseDate))
                {
                    otherExpenseViewModelList = otherExpenseViewModelList != null ? otherExpenseViewModelList.Where(o => o.ExpenseDate == ExpenseDate).ToList():null;
                }
                if(!string.IsNullOrEmpty(DefaultDate))
                {
                    ExpenseDate = DateTime.Now.AddDays(-int.Parse(DefaultDate)).ToString("dd-MMM-yyyy");
                    otherExpenseViewModelList = otherExpenseViewModelList != null ? otherExpenseViewModelList.Where(o => o.ExpenseDate == ExpenseDate).ToList() : null;
                }
                string totamt = _commonBusiness.ConvertCurrency(otherExpenseViewModelList != null ? otherExpenseViewModelList.Sum(o => o.Amount):decimal.Zero);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseViewModelList, TotalAmount= totamt});
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllOtherExpenses

        #region GetExpenseDetailsByID
        [HttpGet]
        public string GetExpenseDetailsByID(string ID)
        {
            try
            {
                OtherExpenseViewModel otherExpenseViewModel = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.GetExpenseDetailsByID(Guid.Parse(ID)));
               
                return JsonConvert.SerializeObject(new { Result = "OK", Record = otherExpenseViewModel});
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetExpenseDetailsByID
        #region InsertUpdateOtherExpense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateOtherExpense(OtherExpenseViewModel otherExpenseViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    otherExpenseViewModel.commonObj = new CommonViewModel();
                    otherExpenseViewModel.commonObj.CreatedBy = "Albert Thomson";
                    otherExpenseViewModel.commonObj.CreatedDate = DateTime.Now;
                    otherExpenseViewModel.commonObj.UpdatedBy = otherExpenseViewModel.commonObj.CreatedBy;
                    otherExpenseViewModel.commonObj.UpdatedDate = otherExpenseViewModel.commonObj.CreatedDate;
                    OtherExpenseViewModel otherExpenseVM = null;

                    switch (otherExpenseViewModel.ID==Guid.Empty)
                    {
                        //INSERT
                        case true:
                            otherExpenseVM = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.InsertOtherExpense(Mapper.Map<OtherExpenseViewModel, OtherExpense>(otherExpenseViewModel)));
                            return JsonConvert.SerializeObject(new { Result = "OK", Record = otherExpenseVM });
                        default:
                            //Getting UA
                            otherExpenseVM = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.UpdateOtherExpense(Mapper.Map<OtherExpenseViewModel, OtherExpense>(otherExpenseViewModel)));
                            return JsonConvert.SerializeObject(new { Result = "OK", Record = otherExpenseVM });
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            //Model state errror
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
            }
        }
        #endregion InsertUpdateOtherExpense
        #region DeleteOtherExpense
        
        public string DeleteOtherExpense(string ID)
        {

            try
            {
                object result = null;

                result = _otherExpenseBusiness.DeleteOtherExpense(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherExpense
     
        #region ButtonStyling
        [HttpGet]
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



                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "saveNow();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    break;
                case "Add":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddOtherExpense();";


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