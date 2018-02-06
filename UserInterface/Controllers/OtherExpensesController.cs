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
        #region Constructor Injection
        // GET: OtherExpenses
        AppConst c = new AppConst();
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        IEmployeeBusiness _employeeBusiness;
        IApprovalStatusBusiness _approvalStatusBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public OtherExpensesController(IOtherExpenseBusiness otherExpenseBusiness, ICommonBusiness commonBusiness,IEmployeeBusiness employeeBusiness,IApprovalStatusBusiness approvalStatusBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
            _employeeBusiness = employeeBusiness;
            _approvalStatusBusiness = approvalStatusBusiness;
            _tool = tool;
        }
        #endregion Constructor Injection

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public ActionResult Index(string id)
        {
            OtherExpenseViewModel otherExpenseViewModelObj = null;
            ViewBag.value = id;
            Settings s = new Settings();
            try
            {
                otherExpenseViewModelObj = new OtherExpenseViewModel();
                otherExpenseViewModelObj.ExpenseDateFormatted = DateTime.Today.ToString(s.dateformat);
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

                otherExpenseViewModelObj.ApprovalStatusList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<ApprovalStatusViewModel> approvalStatus = Mapper.Map<List<ApprovalStatus>, List<ApprovalStatusViewModel>>(_approvalStatusBusiness.GetAllApprovalStatus());
                foreach (ApprovalStatusViewModel BL in approvalStatus)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = BL.Description,
                        Value = BL.Code,
                        Selected = false
                    });
                }
                otherExpenseViewModelObj.ApprovalStatusList = selectListItem;

                Permission permission = Session["UserRights"] as Permission;
                string p = permission.SubPermissionList.Where(li => li.Name == "DaysFilter").First().AccessCode;
                if (p.Contains("R") || p.Contains("W")) {
                    otherExpenseViewModelObj.ShowDaysFilter = true;
                }
                else{
                    otherExpenseViewModelObj.ShowDaysFilter = false;
                }
                p = null;
                p = permission.SubPermissionList.Where(li => li.Name == "ApprovalFilter").First().AccessCode;
                if (p.Contains("R") || p.Contains("W"))
                {
                    otherExpenseViewModelObj.ApprovalFilter = true;
                }
                else
                {
                    otherExpenseViewModelObj.ApprovalFilter = false;
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

        #region GetOpeningBalance
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetOpeningBalance(string OpeningDate)
        {
            try
            {
                 OtherExpenseViewModel otherExpenseObj = Mapper.Map<OtherExpense,OtherExpenseViewModel>(_otherExpenseBusiness.GetOpeningBalance(OpeningDate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllEmployeeTypes

        #region GetBankWiseBalance
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetBankWiseBalance(string Date)
        {
            try
            {
                List<OtherExpenseViewModel> otherExpenseList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetBankWiseBalance(Date));
                string TotalAmountFormatted = _commonBusiness.ConvertCurrency(otherExpenseList.Sum(OE => OE.TotalAmount), 2);
                string TotalUnClrAmtFormatted = _commonBusiness.ConvertCurrency(otherExpenseList.Sum(OE => OE.UnClearedAmount), 2);
                string ActualBlnceFormatted = _commonBusiness.ConvertCurrency(otherExpenseList.Sum(OE => OE.TotalAmount)+ otherExpenseList.Sum(OE => OE.UnClearedAmount), 2);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseList,TotalAmount= TotalAmountFormatted, TotalUnClrAmt= TotalUnClrAmtFormatted, ActualBlnce= ActualBlnceFormatted });
            }
            catch (Exception ex)
            {
               
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetBankWiseBalance
        
        #region GetEmployeeCompanyDetails
        public string GetEmployeeCompanyDetails(string ID)
        {
            try
            {
                List<EmployeeViewModel> employeeViewModelList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_otherExpenseBusiness.GetCompanybyEmployee(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = employeeViewModelList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetEmployeeCompanyDetails

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
               else if(!string.IsNullOrEmpty(DefaultDate))
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

        #region GetReversalReference
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetReversalReference(string EmpID, string AccountCode, string EmpTypeCode)
        {
            try
            {
                List<OtherExpenseViewModel> otherExpenseViewModelList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetReversalReference(EmpID,AccountCode,EmpTypeCode));
                
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseViewModelList });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetReversalReference

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

                     AppUA appUA = Session["AppUA"] as AppUA;
                    otherExpenseViewModel.commonObj = new CommonViewModel();
                    SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
                    otherExpenseViewModel.commonObj.CreatedBy = appUA.UserName;
                    otherExpenseViewModel.commonObj.CreatedDate = appUA.DateTime;
                    otherExpenseViewModel.commonObj.UpdatedBy = appUA.UserName;
                    otherExpenseViewModel.commonObj.UpdatedDate = common.GetCurrentDateTime();
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


                AppUA appUA = Session["AppUA"] as AppUA;
                _employeeObj.commonObj = new CommonViewModel();
                SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
                _employeeObj.commonObj.CreatedBy = appUA.UserName;
                _employeeObj.commonObj.CreatedDate = appUA.DateTime;
                _employeeObj.commonObj.UpdatedBy = appUA.UserName;
                _employeeObj.commonObj.UpdatedDate = common.GetCurrentDateTime();

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
                AppUA appUA = Session["AppUA"] as AppUA;
                result = _otherExpenseBusiness.DeleteOtherExpense(Guid.Parse(ID), appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherExpense

        #region GetMaximumReducibleAmount
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetMaximumReducibleAmount(string refNumber)
        {
            try
            {
                decimal amount = _otherExpenseBusiness.GetMaximumReducibleAmount(refNumber);
                return JsonConvert.SerializeObject(new { Result = "OK", ReducibleAmount = amount });
            }
            catch(Exception Ex)
            {
                AppConstMessage cm = c.GetMessage(Ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetMaximumReducableAmount

        #region GetValueFromSettings
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetValueFromSettings()
        {
            try
            {
                SysSettings sysSettings = new SysSettings();
                sysSettings.Name        = "OE-LIMIT";
                sysSettings.Value = _otherExpenseBusiness.GetValueFromSettings(sysSettings);
                return JsonConvert.SerializeObject(new { Result = "OK", sysSettingsObj = sysSettings });
            }
            catch(Exception Ex)
            {
                AppConstMessage cm = c.GetMessage(Ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetValueFromSettings

        #region UpdateValueInSettings
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string UpdateValueInSettings(string Value)
        {
            try
            {
                AppUA appUA            = Session["AppUA"] as AppUA;
                SysSettingsViewModel sysSettings = new SysSettingsViewModel();
                sysSettings.Name        = "OE-LIMIT";
                sysSettings.Value       = Value;
                sysSettings.CommonObj = new CommonViewModel();
                SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
                sysSettings.CommonObj.UpdatedBy     = appUA.UserName;
                sysSettings.CommonObj.UpdatedDate   = common.GetCurrentDateTime();
                string actionMessage = _otherExpenseBusiness.UpdateValueInSettings(Mapper.Map<SysSettingsViewModel,SysSettings>(sysSettings));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = actionMessage });
            }
            catch(Exception Ex)
            {
                AppConstMessage cm = c.GetMessage(Ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion UpdateValueInSettings

        #region GetAllOtherExpenseByApprovalStatus
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetAllOtherExpenseByApprovalStatus(string Status,string ExpenseDate,string DefaultDate)
        {
            try
            {
                List<OtherExpense> otherExpenseList = _otherExpenseBusiness.GetAllOtherExpenseByApprovalStatus((Status!="")?int.Parse(Status):0, ExpenseDate);
                if (!string.IsNullOrEmpty(DefaultDate))
                {
                    if (DefaultDate == "0")
                    {
                        otherExpenseList = otherExpenseList != null ? otherExpenseList
                       .ToList() : null;
                    }
                    else
                    {
                        ExpenseDate = DateTime.Now.AddDays(-int.Parse(DefaultDate)).ToString("dd-MMM-yyyy");
                        otherExpenseList = otherExpenseList != null ? otherExpenseList
                        .Where(o => DateTime.Parse(o.ExpenseDate).Date >= DateTime.Parse(ExpenseDate).Date && DateTime.Parse(o.ExpenseDate).Date <= DateTime.Now.Date)
                        .ToList() : null;
                    }
                }
                string sumAmount = _commonBusiness.ConvertCurrency(otherExpenseList != null ? otherExpenseList.Sum(o => o.Amount):decimal.Zero);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseList, TotalAmount = sumAmount });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllOtherExpenseByApprovalStatus

        #region SendNotification
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "W")]
        [HttpPost]
        public string SendNotification(OtherExpenseViewModel otherExpenseVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                SPAccounts.DataAccessObject.DTO.Common common = new SPAccounts.DataAccessObject.DTO.Common();
                otherExpenseVM.commonObj = new CommonViewModel();
                otherExpenseVM.commonObj.UpdatedBy = appUA.UserName;
                otherExpenseVM.commonObj.UpdatedDate = common.GetCurrentDateTime();
                OtherExpenseViewModel result = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.UpdateOtherExpense(Mapper.Map<OtherExpenseViewModel, OtherExpense>(otherExpenseVM)));

                string titleString = "Expense Approval";
                string descriptionString = otherExpenseVM.RefNo + ", Expense: " + otherExpenseVM.AccountCode + ", Amount: " + otherExpenseVM.Amount + ", Notes: " + otherExpenseVM.Description;
                Boolean isCommon = true;
                string customerID = "";
                _commonBusiness.SendToFCM(titleString, descriptionString, isCommon, customerID);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.NotificationSuccess, Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion SendNotification

        #region PayOtherExpense
        public string PayOtherExpense(string ID)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string returnMessage = _otherExpenseBusiness.PayOtherExpense(Guid.Parse(ID), appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = returnMessage });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion PayOtherExpense

        #region validaterefno

        public string Validate(OtherExpenseViewModel _otherexpenseObj)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            object result = null;
            try
            {
                result = _otherExpenseBusiness.Validate(Mapper.Map<OtherExpenseViewModel, OtherExpense>(_otherexpenseObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = "", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message, Status = -1 });
            }
        }
        #endregion validaterefno

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            Permission permission = Session["UserRights"] as Permission;
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

                    ToolboxViewModelObj.LimitBtn.Visible = true;
                    ToolboxViewModelObj.LimitBtn.Text = "Limit";
                    ToolboxViewModelObj.LimitBtn.Title = "Max Limit On Amount";
                    ToolboxViewModelObj.LimitBtn.Event = "openLimitModal();";

                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, permission);

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