using AutoMapper;
using Newtonsoft.Json;
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
    public class OtherExpensesController : Controller
    {
        // GET: OtherExpenses
        AppConst c = new AppConst();
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        IEmployeeBusiness _employeeBusiness;
        public OtherExpensesController(IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness,IEmployeeBusiness employeeBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
            _employeeBusiness = employeeBusiness;
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
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
                        Value = cav.Code +":"+ cav.ISEmploy,
                        Selected = false,
                      
                        
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
                //selectListItem = new List<SelectListItem>();
                //List<EmployeeViewModel> empList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployees());
                //foreach (EmployeeViewModel evm in empList)
                //{
                //    selectListItem.Add(new SelectListItem
                //    {
                //        Text = evm.Name,
                //        Value = evm.ID.ToString(),
                //        Selected = false
                //    });
                //}
                //otherExpenseViewModelObj.EmployeeList = selectListItem;
                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                List<EmployeeTypeViewModel> empTypeList = Mapper.Map<List<EmployeeType>, List<EmployeeTypeViewModel>>(_otherExpenseBusiness.GetAllEmployeeTypes());
                foreach (EmployeeTypeViewModel etvm in empTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = etvm.Name,
                        Value = etvm.Code,
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.EmployeeTypeList = selectListItem;



                Permission _permission = Session["UserRights"] as Permission;
                string p = _permission.SubPermissionList.Where(li => li.Name == "DaysFilter").First().AccessCode;
                if (p.Contains("R") || p.Contains("W")) {
                    otherExpenseViewModelObj.ShowDaysFilter = true;
                }
                else{
                    otherExpenseViewModelObj.ShowDaysFilter = false;
                }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(otherExpenseViewModelObj);
            
        }

        #region GetAllEmployeeTypes
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetAllEmployeesByType(string Type)
        {
            try
            {
                List<EmployeeViewModel> employeeViewModelList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetAllEmployeesByType(Type));
                var empList = employeeViewModelList != null ? employeeViewModelList.Select(i => new { i.ID, i.Name }).ToList() : null;
                return JsonConvert.SerializeObject(new { Result = "OK", Records = empList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllEmployeeTypes

        #region GetAllOtherExpenses
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetAllOtherExpenses(string ExpenseDate, string DefaultDate)
        {
            try
            {
                List<OtherExpenseViewModel> otherExpenseViewModelList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetAllOtherExpenses());
                if(!string.IsNullOrEmpty(ExpenseDate))
                {

                    otherExpenseViewModelList = otherExpenseViewModelList != null ? otherExpenseViewModelList
                     .Where(o => o.ExpenseDate == ExpenseDate)
                    .ToList():null;
                }
                if(!string.IsNullOrEmpty(DefaultDate))
                {
                    if (DefaultDate == "0")
                    {
                        otherExpenseViewModelList = otherExpenseViewModelList != null ? otherExpenseViewModelList                      
                       .ToList() : null;
                    }
                    else
                    {
                        //DefaultDate is no. of days coming ie 30 or 60 days
                        ExpenseDate = DateTime.Now.AddDays(-int.Parse(DefaultDate)).ToString("dd-MMM-yyyy");
                        //JobList = JobList == null ? null : JobList.Where(stype => stype.SCCode == SCCode && stype.Employee.ID == id && DateTime.Parse(stype.ServiceDate) == DateTime.Parse(servicedate)).ToList();
                        otherExpenseViewModelList = otherExpenseViewModelList != null ? otherExpenseViewModelList
                        .Where(o => DateTime.Parse(o.ExpenseDate).Date >= DateTime.Parse(ExpenseDate).Date && DateTime.Parse(o.ExpenseDate).Date <= DateTime.Now.Date)
                        .ToList() : null;
                    }
                   

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
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetExpenseDetailsByID(string ID)
        {
            try
            {
                OtherExpenseViewModel otherExpenseViewModel = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.GetExpenseDetailsByID(Guid.Parse(ID)));
                if(otherExpenseViewModel!=null)
                {
                    otherExpenseViewModel.AccountCode = otherExpenseViewModel.AccountCode + ":" + otherExpenseViewModel.chartOfAccountsObj.ISEmploy;
                }
               
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
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "W")]
        public string InsertUpdateOtherExpense(OtherExpenseViewModel otherExpenseViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //removiing combined code
                    int len=otherExpenseViewModel.AccountCode.IndexOf(':');
                    otherExpenseViewModel.AccountCode = otherExpenseViewModel.AccountCode.Remove(len);
                    //

                     AppUA _appUA = Session["AppUA"] as AppUA;
                    otherExpenseViewModel.commonObj = new CommonViewModel();
                    otherExpenseViewModel.commonObj.CreatedBy = _appUA.UserName;
                    otherExpenseViewModel.commonObj.CreatedDate = _appUA.DateTime;
                    otherExpenseViewModel.commonObj.UpdatedBy = _appUA.UserName;
                    otherExpenseViewModel.commonObj.UpdatedDate = _appUA.DateTime;
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

        #region InsertUpdateEmployee
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "W")]
        public string InsertUpdateEmployee(EmployeeViewModel _employeeObj)
        {
            object result = null;

            try
            {


                AppUA _appUA = Session["AppUA"] as AppUA;
                _employeeObj.commonObj = new CommonViewModel();
                _employeeObj.commonObj.CreatedBy = _appUA.UserName;
                _employeeObj.commonObj.CreatedDate = _appUA.DateTime;
                _employeeObj.commonObj.UpdatedBy = _appUA.UserName;
                _employeeObj.commonObj.UpdatedDate = _appUA.DateTime;

                result = _employeeBusiness.InsertUpdateEmployee(Mapper.Map<EmployeeViewModel, Employee>(_employeeObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion InsertUpdateEmployee

        #region DeleteOtherExpense
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "D")]
        public string DeleteOtherExpense(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _otherExpenseBusiness.DeleteOtherExpense(Guid.Parse(ID),_appUA.UserName);
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
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
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