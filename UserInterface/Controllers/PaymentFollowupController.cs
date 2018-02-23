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
    public class PaymentFollowupController : Controller
    {
        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;
        ICommonBusiness _commonBusiness;
        IPaymentFollowupBusiness _paymentFollowupBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public PaymentFollowupController(IOtherExpenseBusiness otherExpenseBusiness, ICustomerBusiness customerBusiness, IPaymentFollowupBusiness paymentFollowupBusiness, SecurityFilter.ToolBarAccess tool, ICommonBusiness commonBusiness)
        {

            _otherExpenseBusiness = otherExpenseBusiness;
            _commonBusiness = commonBusiness;
            _paymentFollowupBusiness = paymentFollowupBusiness;
             _customerBusiness = customerBusiness;
            _tool = tool;
            
        }
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.value = id;
            AppUA appUA = Session["AppUA"] as AppUA;
            DateTime dt = new DateTime();
            dt = appUA.DateTime;
            ViewBag.toDate = dt.ToString("dd-MMM-yyyy");
            CustomerExpeditingListViewModel result = new CustomerExpeditingListViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem { Text = "--Select--", Value = "ALL", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "Coming Week", Value = "ThisWeek", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "Today", Value = "Today", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "1-30 Days", Value = "1To30", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "31-60 Days", Value = "31To60", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "61-90 Days", Value = "61To90", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "90 Above", Value = "90Above", Selected = false });

            if (id == null || id == "")
            {
                var selected = selectListItem.Where(x => x.Value == "ALL").First();
                selected.Selected = true;
            }
            else
            {
                try
                {
                    var selected = selectListItem.Where(x => x.Value == id).First();
                    selected.Selected = true;
                }
                catch (Exception)
                {
                    result.Filter = "ALL";
                }

            }
            result.BasicFilters = selectListItem;

             selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem { Text = "Outstanding", Value = "Outstanding", Selected = true });
            selectListItem.Add(new SelectListItem { Text = "All", Value = "All", Selected = false });
            result.Outstanding = selectListItem;

            selectListItem = new List<SelectListItem>();
            result.customerObj = new CustomerViewModel();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                foreach (CustomerViewModel customerVM in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = customerVM.CompanyName,
                        Value = customerVM.CompanyName.ToString(),
                        Selected = false
                    });
                }
            }
            result.customerObj.CustomerList = selectListItem;

            selectListItem = new List<SelectListItem>();
            result.companyObj = new CompaniesViewModel();
            List<CompaniesViewModel> companiesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_otherExpenseBusiness.GetAllCompanies());
            if (companiesList != null)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = "All",
                    Value = "ALL",
                    Selected = true
                });

                foreach (CompaniesViewModel companiesVM in companiesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = companiesVM.Name,
                        Value = companiesVM.Name.ToString(),
                        Selected = false
                    });
                }
            }
            result.companyObj.CompanyList = selectListItem;
            return View(result);
        }
        #region GetCustomerList
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public string GetCustomerPaymentExpeditingDetails(string toDate, string filter, string company, string[] customer,string outstanding,string search)
        {
            try
            {
                DateTime? TDate = string.IsNullOrEmpty(toDate) ? (DateTime?)null : DateTime.Parse(toDate);
                CustomerExpeditingListViewModel result = new CustomerExpeditingListViewModel();
                result.customerExpeditingDetailsList = Mapper.Map<List<CustomerExpeditingReport>, List<CustomerExpeditingReportViewModel>>(_paymentFollowupBusiness.GetCustomerExpeditingDetail(TDate, filter, company,customer != null ? string.Join(",", customer) : "ALL", outstanding,search));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetCustomerList

        #region GetFollowupCount
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public string GetRecentFollowUpCount()
        {
            try
            {
                //AppUA _appUA = Session["AppUAOffice"] as AppUA;
                // DateTime? Date = string.IsNullOrEmpty(_appUA.DateTime) ? (DateTime?)null : DateTime.Parse(Today);
                SPAccounts.DataAccessObject.DTO.Common comonObj = new SPAccounts.DataAccessObject.DTO.Common();
                List<FollowUpViewModel> followupObj = Mapper.Map<List<FollowUp>, List<FollowUpViewModel>>(_paymentFollowupBusiness.GetRecentFollowUpCount(comonObj.GetCurrentDateTime()));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = followupObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetFollowupCount

        #region Followup
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public ActionResult Followup(FollowUpViewModel followObj)
        {
            List<FollowUpViewModel> followUpObj = Mapper.Map<List<FollowUp>, List<FollowUpViewModel>>(_paymentFollowupBusiness.GetFollowUpDetails(followObj.CustomerID != null && followObj.CustomerID.ToString() != "" ? Guid.Parse(followObj.CustomerID.ToString()) : Guid.Empty));
            int openCount = followUpObj == null ? 0 : followUpObj.Where(Q => Q.Status == "Open").Select(T => T.ID).Count();
            FollowUpListViewModel Result = new FollowUpListViewModel();
            Result.FollowUpList = followUpObj;
            Result.FlwID = followObj.ID;
            ViewBag.Count = openCount;
            return PartialView("_FollowUpList",Result);
        }
        #endregion Followup

        #region InsertFollowUp
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public string InsertUpdateFollowUp(CustomerExpeditingListViewModel customerObj)
        {
            try
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                customerObj.followUpObj.commonObj = new CommonViewModel();
                SPAccounts.DataAccessObject.DTO.Common _comonObj = new SPAccounts.DataAccessObject.DTO.Common();
                customerObj.followUpObj.commonObj.CreatedBy = _appUA.UserName;
                customerObj.followUpObj.commonObj.CreatedDate = _comonObj.GetCurrentDateTime();
                customerObj.followUpObj.commonObj.UpdatedDate = _comonObj.GetCurrentDateTime();
                customerObj.followUpObj.commonObj.UpdatedBy = _appUA.UserName;
                FollowUpViewModel followupObj = Mapper.Map<FollowUp, FollowUpViewModel>(_paymentFollowupBusiness.InsertUpdateFollowUp(Mapper.Map<FollowUpViewModel, FollowUp>(customerObj.followUpObj)));

                if (customerObj.followUpObj.ID == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = followupObj, Message = "Insertion successfull" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = followupObj, Message = "Updation successfull" });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }
        #endregion

        #region GetFollowUpDetailByFollowUpId
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public string GetFollowUpDetailByFollowUpId(Guid ID)
        {
            FollowUpViewModel followupObj = Mapper.Map<FollowUp, FollowUpViewModel>(_paymentFollowupBusiness.GetFollowupDetailsByFollowUpID(ID != null && ID.ToString() != "" ? Guid.Parse(ID.ToString()) : Guid.Empty));

            return JsonConvert.SerializeObject(new { Result = "OK", Records = followupObj });

        }
        #endregion GetFollowUpDetailByFollowUpId

        #region DeleteFollowUp
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "D")]
        public string DeleteFollowUp(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _paymentFollowupBusiness.DeleteFollowUp(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = c.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion DeleteFollowUp

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PaymentFollowups", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            Permission _permission = Session["UserRights"] as Permission;

            switch (actionType)
            {
                case "List":
                   

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";

                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);
                    break;
                case "CustDetail":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "closeNav();";
                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);
                    break;

                case "ListWithReset":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Disable = false;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";


                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

                    break;

                case "ListWithPrint":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.downloadBtn.Visible = true;
                    ToolboxViewModelObj.downloadBtn.Text = "Download";
                    ToolboxViewModelObj.downloadBtn.Title = "Download";
                    ToolboxViewModelObj.downloadBtn.Event = "DownloadReport();";

                    ToolboxViewModelObj = _tool.SetToolbarAccess(ToolboxViewModelObj, _permission);

                    break;


                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}