﻿using AutoMapper;
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
    public class DepositAndWithdrawalsController : Controller
    {

        #region Constructor_Injection 

        AppConst c = new AppConst();
        IDepositAndWithdrawalsBusiness _depositAndWithdrawalsBusiness;
        IBankBusiness _bankBusiness;
        ICommonBusiness _commonBusiness;
        IPaymentModesBusiness _paymentModesBusiness;
        ICustomerBusiness _customerBusiness;
        ICompaniesBusiness _companiesBusiness;
        IOtherExpenseBusiness _otherExpenseBusiness;

        public DepositAndWithdrawalsController(IDepositAndWithdrawalsBusiness depositAndWithdrawalsBusiness, IBankBusiness bankBusiness, ICommonBusiness commonBusiness, IPaymentModesBusiness paymentModesBusiness, ICustomerBusiness customerBusiness, ICompaniesBusiness companiesBusiness,IOtherExpenseBusiness otherExpenseBusiness)
        {
            _depositAndWithdrawalsBusiness = depositAndWithdrawalsBusiness;
            _bankBusiness = bankBusiness;
            _commonBusiness = commonBusiness;
            _paymentModesBusiness = paymentModesBusiness;
            _customerBusiness = customerBusiness;
            _companiesBusiness = companiesBusiness;
            _otherExpenseBusiness = otherExpenseBusiness;
        }
        #endregion Constructor_Injection 

        // GET: DepositAndWithdrawals
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public ActionResult Index()
        {
            DepositAndWithdrwalViewModel depositAndWithdrwalViewModelObj = null;
            try
            {
                depositAndWithdrwalViewModelObj = new DepositAndWithdrwalViewModel();
                depositAndWithdrwalViewModelObj.bankList = new List<SelectListItem>();


                List<SelectListItem> selectListItem = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<BankViewModel> PayTermList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
                foreach (BankViewModel bvm in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = bvm.Name,
                        Value = bvm.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.bankList = selectListItem;

                depositAndWithdrwalViewModelObj.paymentModeList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<PaymentModesViewModel> PaymentList = Mapper.Map<List<PaymentModes>, List<PaymentModesViewModel>>(_paymentModesBusiness.GetAllPaymentModes());
                foreach (PaymentModesViewModel PMVM in PaymentList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PMVM.Description,
                        Value = PMVM.Code,
                        Selected = false
                    });
                }

                depositAndWithdrwalViewModelObj.paymentModeList = selectListItem;

                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
                if (customerList != null)
                {
                    foreach (CustomerViewModel Cust in customerList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Text = Cust.CompanyName,
                            Value = Cust.ID.ToString(),
                            Selected = false
                        });
                    }
                }
                depositAndWithdrwalViewModelObj.customerList = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(depositAndWithdrwalViewModelObj);
        }

        #region GetAllDepositAndWithdrawals
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public string GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal, string chqclr)
        {
            try
            {

                List<DepositAndWithdrwalViewModel> depositAndWithdrwalsList = Mapper.Map<List<DepositAndWithdrawals>, List<DepositAndWithdrwalViewModel>>(_depositAndWithdrawalsBusiness.GetAllDepositAndWithdrawals(FromDate, ToDate, DepositOrWithdrawal, chqclr));
                var totalWdl = depositAndWithdrwalsList.Where(amt => amt.TransactionType == "W").Sum(amt => amt.Amount);
                var totalDpt = depositAndWithdrwalsList.Where(amt => amt.TransactionType == "D").Sum(amt => amt.Amount);
                string totalWdlFormatted = _commonBusiness.ConvertCurrency(totalWdl, 2);
                string totalDptFormatted = _commonBusiness.ConvertCurrency(totalDpt, 2);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalsList, totalWdl = totalWdlFormatted, totalDpt = totalDptFormatted });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllDepositAndWithdrawals

        #region GetDepositAndWithdrawalDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public string GetDepositAndWithdrawalDetails(string ID)
        {
            try
            {

                DepositAndWithdrwalViewModel depositAndWithdrwalObj = Mapper.Map<DepositAndWithdrawals, DepositAndWithdrwalViewModel>(_depositAndWithdrawalsBusiness.GetDepositAndWithdrawalDetails(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetDepositAndWithdrawalDetails

        #region GetTransferCashByIdDetails
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public string GetTransferCashById(string TransferId)
        {
            try
            {

                DepositAndWithdrwalViewModel depositAndWithdrwalObj = Mapper.Map<DepositAndWithdrawals, DepositAndWithdrwalViewModel>(_depositAndWithdrawalsBusiness.GetTransferCashById(TransferId != null && TransferId != "" ? Guid.Parse(TransferId) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetTransferCashByIdDetails

        #region InsertUpdateDepositAndWithdrawals
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "W")]
        public string InsertUpdateDepositAndWithdrawals(DepositAndWithdrwalViewModel _depositAndWithdrwalObj)
        {
            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _depositAndWithdrwalObj.CommonObj = new CommonViewModel();
                _depositAndWithdrwalObj.CommonObj.CreatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.CreatedDate = _appUA.DateTime;
                _depositAndWithdrwalObj.CommonObj.UpdatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.UpdatedDate = _appUA.DateTime;

                if (_depositAndWithdrwalObj.DepositRowValues != null && _depositAndWithdrwalObj.DepositRowValues != "0")
                {
                    _depositAndWithdrwalObj.CheckedRows = JsonConvert.DeserializeObject<List<DepositAndWithdrwalViewModel>>(_depositAndWithdrwalObj.DepositRowValues);//.Select(c => { c.Password = null; return c; }).ToList();
                    _depositAndWithdrwalObj.CheckedRows = _depositAndWithdrwalObj.CheckedRows == null ? null : _depositAndWithdrwalObj.CheckedRows.Select(x => { x.CommonObj = new CommonViewModel { CreatedBy = _appUA.UserName, CreatedDate = _appUA.DateTime }; return x; }).ToList();

                }
                //Author:Praveena M S
                //While updating bank ,hidden field values 'Status and ChequeClearDate' are taken and assigned to ChequeClearDate and ChequeStatus for avoiding null entries......
                if (_depositAndWithdrwalObj.hdnChequeStatus == "Cleared")
                {
                    if (_depositAndWithdrwalObj.ChequeClearDate == null && _depositAndWithdrwalObj.ChequeStatus == null)
                    {
                        _depositAndWithdrwalObj.ChequeClearDate = _depositAndWithdrwalObj.hdnChequeDate;
                        _depositAndWithdrwalObj.ChequeStatus = _depositAndWithdrwalObj.hdnChequeStatus;
                    }
                }
                result = _depositAndWithdrawalsBusiness.InsertUpdateDepositAndWithdrawals(Mapper.Map<DepositAndWithdrwalViewModel, DepositAndWithdrawals>(_depositAndWithdrwalObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateDepositAndWithdrawals

        #region DeleteDepositAndWithdrawals
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "D")]
        public string DeleteDepositandwithdrawal(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _depositAndWithdrawalsBusiness.DeleteDepositandwithdrawal(Guid.Parse(ID), _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteDepositAndWithdrawals

        
        #region DeleteTransferAmountBetweenBanks
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "D")]
        public string DeleteTransferAmount(string TransferID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _depositAndWithdrawalsBusiness.DeleteTransferAmount(Guid.Parse(TransferID), _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteTransferAmountBetweenBanks


        #region ClearCheque
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "W")]
        public string ClearCheque(string ID, string Date)
        {
            try
            {
                object result = null;

                result = _depositAndWithdrawalsBusiness.ClearCheque(ID, Date);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ClearCheque

        #region GetUndepositedCheque

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "UndepositedCheque", Mode = "R")]
        public ActionResult undeposited()
        {
            DateTime dt = DateTime.Now;
            ViewBag.fromdate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");

            DepositAndWithdrwalViewModel result = new DepositAndWithdrwalViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            result.CompanyObj = new CompaniesViewModel();
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
            result.CompanyObj.CompanyList = selectListItem;


            selectListItem = new List<SelectListItem>();
            result.CustomerObj = new CustomerViewModel();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
            if (customerList != null)
            {
                //selectListItem.Add(new SelectListItem
                //{
                //    Text = "All",
                //    Value = "ALL",
                //    Selected = true
                //});

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
            result.CustomerObj.CustomerList = selectListItem;
            selectListItem = new List<SelectListItem>();
            result.BankObj = new BankViewModel();
            List<BankViewModel> PayTermList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
            foreach (BankViewModel bankvm in PayTermList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = bankvm.Name,
                    Value = bankvm.Name,
                    Selected = false
                });
            }
            result.BankObj.BanksList = selectListItem;
            return View(result);
        }

       
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "UndepositedCheque", Mode = "R")]
        public string GetUndepositedCheque(string undepositedChequeAdvanceSearchObject)//FromDate, string ToDate)
        {
            try
            {                
                //if (fromDate != "" ? Convert.ToDateTime(fromDate) > Convert.ToDateTime(chequeAdvanceSearchObj.ToDate) : false)
                //{
                //    throw new Exception("Date missmatch");
                //}
                AppUA appUA = Session["AppUA"] as AppUA;
                UndepositedChequeAdvanceSearch undepositedChequeAdvanceSearchObj = undepositedChequeAdvanceSearchObject != null ? JsonConvert.DeserializeObject<UndepositedChequeAdvanceSearch>(undepositedChequeAdvanceSearchObject) : new UndepositedChequeAdvanceSearch();
                if (undepositedChequeAdvanceSearchObject == null)
                {
                    undepositedChequeAdvanceSearchObj.ToDate = appUA.DateTime.ToString("dd-MMM-yyyy");
                }
                List<DepositAndWithdrwalViewModel> unDepositedChequeList = Mapper.Map<List<DepositAndWithdrawals>, List<DepositAndWithdrwalViewModel>>(_depositAndWithdrawalsBusiness.GetUndepositedCheque(undepositedChequeAdvanceSearchObj));
                string MinDate = unDepositedChequeList.Count != 0 ? Convert.ToDateTime((unDepositedChequeList.Min(X => Convert.ToDateTime(X.DateFormatted)))).ToString("dd-MMM-yyyy") : undepositedChequeAdvanceSearchObj.FromDate;               
                var totalAmount = unDepositedChequeList.Where(amt => amt.TransactionType != "D").Sum(amt => amt.Amount);
                string totalAmountFormatted = _commonBusiness.ConvertCurrency(totalAmount, 2);               
                return JsonConvert.SerializeObject(new { Result = "OK", Records = unDepositedChequeList, FromDate = MinDate,totalAmount = totalAmountFormatted });                
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);                
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message});//, FromDate = FromDate });
            }
        }
        #endregion  GetUndepositedCheque

        #region  GetUndepositedChequeCount
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "UndepositedCheque", Mode = "R")]
        public string GetUndepositedChequeCount()
        {
            try
            {
                Common C = new Common();
                string Date = C.GetCurrentDateTime().ToString("dd-MMM-yyyy");
                string unDepositedCount = _depositAndWithdrawalsBusiness.GetUndepositedChequeCount(Date);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = unDepositedCount });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetUndepositedChequeCount

        #region Transfer Cash between banks
        //Insert and Update Cash transferred between banks
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "UndepositedCheque", Mode = "R")]
        public string InsertUpdateTransferAmount(DepositAndWithdrwalViewModel _depositAndWithdrwalObj)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                _depositAndWithdrwalObj.CommonObj = new CommonViewModel();
                _depositAndWithdrwalObj.CommonObj.CreatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.CreatedDate = _appUA.DateTime;
                _depositAndWithdrwalObj.CommonObj.UpdatedBy = _appUA.UserName;
                _depositAndWithdrwalObj.CommonObj.UpdatedDate = _appUA.DateTime;
                result = _depositAndWithdrawalsBusiness.InsertUpdateTransferAmount(Mapper.Map<DepositAndWithdrwalViewModel, DepositAndWithdrawals>(_depositAndWithdrwalObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion Transfer Cash between banks

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":

                    ToolboxViewModelObj.DepositBtn.Visible = true;
                    ToolboxViewModelObj.DepositBtn.Text = "Deposit  ";
                    ToolboxViewModelObj.DepositBtn.Title = "Deposit";
                    ToolboxViewModelObj.DepositBtn.Event = "ShowDepositModal();";

                    ToolboxViewModelObj.WithdrawBtn.Visible = true;
                    ToolboxViewModelObj.WithdrawBtn.Text = "WDL      ";
                    ToolboxViewModelObj.WithdrawBtn.Title = "WithDrawal";
                    ToolboxViewModelObj.WithdrawBtn.Event = "ShowWithDrawal();";

                    ToolboxViewModelObj.ClearBtn.Visible = true;
                    ToolboxViewModelObj.ClearBtn.Text = "ChqClr (In)";
                    ToolboxViewModelObj.ClearBtn.Title = "Cheque Clear";
                    ToolboxViewModelObj.ClearBtn.Event = "ShowChequeClear();";


                    ToolboxViewModelObj.ClearOutBtn.Visible = true;
                    ToolboxViewModelObj.ClearOutBtn.Text = "ChqClr (Out)";
                    ToolboxViewModelObj.ClearOutBtn.Title = "Update Cheque clear date";
                    ToolboxViewModelObj.ClearOutBtn.Event = "ShowChequeClearOut();";

                    ToolboxViewModelObj.TransferBtn.Visible = true;
                    ToolboxViewModelObj.TransferBtn.Text = "Transfer  ";
                    ToolboxViewModelObj.TransferBtn.Title = "CashTransfer";
                    ToolboxViewModelObj.TransferBtn.Event = "ShowCashTransfer();";

                    break;

                case "ListWithReset":
                    //ToolboxViewModelObj.backbtn.Visible = true;
                    //ToolboxViewModelObj.backbtn.Disable = false;
                    //ToolboxViewModelObj.backbtn.Text = "Back";
                    //ToolboxViewModelObj.backbtn.DisableReason = "Not applicable";
                    //ToolboxViewModelObj.backbtn.Event = "Back();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;

                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "ShowModal();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Other Income";
                    ToolboxViewModelObj.savebtn.Event = "Save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Other Income";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;

                case "Export":

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";


                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

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

                    break;
                case "AddNew":
                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintReport();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "FilterReset();";

                    break;
                case "AddSub":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveForm();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew()";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Outgoing Cheques";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteOutgoingCheque()";

                    break;
                case "AddSubIncoming":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveForm();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew()";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Incoming Cheques";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteOutgoingCheque()";

                    break;

                case "EditOutGoing":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveForm();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew()";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Outgoing Cheques";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteOutgoingCheque()";

                    break;

                case "EditIncoming":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveForm();";

                    ToolboxViewModelObj.CloseBtn.Visible = true;
                    ToolboxViewModelObj.CloseBtn.Text = "Close";
                    ToolboxViewModelObj.CloseBtn.Title = "Close";
                    ToolboxViewModelObj.CloseBtn.Event = "closeNav();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddNew()";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Incoming Cheques";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteOutgoingCheque()";

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

        #region BankBalance
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "R")]
        public ActionResult BankBalance()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            ViewBag.Currentdate = _appUA.DateTime.ToString("dd-MMM-yyyy");
            return View();
        }
        #endregion

        #region ClearCheque Out
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "W")]
        public string ClearChequeOut(string ID, string Date)
        {
            try
            {
                object result = null;

                result = _depositAndWithdrawalsBusiness.ClearChequeOut(ID, Date);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ClearCheque Out

        #region GetAllWithdrawals
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DepositAndWithdrawals", Mode = "W")]
        public string GetAllWithdrawals()
        {
            List<DepositAndWithdrwalViewModel> depositAndWithdrwalsList = Mapper.Map<List<DepositAndWithdrawals>, List<DepositAndWithdrwalViewModel>>(_depositAndWithdrawalsBusiness.GetAllWithdrawals());
            return JsonConvert.SerializeObject(new { Result = "OK", Records = depositAndWithdrwalsList });
        }
        #endregion GetAllWithdrawals


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "R")]
        public ActionResult OutgoingCheques()
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            DateTime dateTime = appUA.DateTime;
            ViewBag.fromdate = dateTime.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dateTime.ToString("dd-MMM-yyyy");
            DepositAndWithdrwalViewModel depositAndWithdrwalViewModelObj = null;
            try
            {
                depositAndWithdrwalViewModelObj = new DepositAndWithdrwalViewModel();
                depositAndWithdrwalViewModelObj.bankList = new List<SelectListItem>();


                List<SelectListItem> selectListItem = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<BankViewModel> PayTermList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
                foreach (BankViewModel bvm in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = bvm.Name,
                        Value = bvm.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.bankList = selectListItem;

                depositAndWithdrwalViewModelObj.CompanyList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
                foreach (CompaniesViewModel Cmp in CompaniesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cmp.Name,
                        Value = Cmp.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.CompanyList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(depositAndWithdrwalViewModelObj);
        }

        #region To List all Outgoingcheques
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "R")]
        public string GetOutGoingCheques(string outGoingChequesAdvanceSearchObject)
        {

            try
            {
                OutgoingChequeAdvanceSearch outAdvancedSearchObj = outGoingChequesAdvanceSearchObject != null ? JsonConvert.DeserializeObject<OutgoingChequeAdvanceSearch>(outGoingChequesAdvanceSearchObject) : new OutgoingChequeAdvanceSearch();
                //DateTime? FDate = string.IsNullOrEmpty(OutAdvancedSearchObj.FromDate) ? (DateTime?)null : DateTime.Parse(OutAdvancedSearchObj.FromDate);
                //DateTime? TDate = string.IsNullOrEmpty(OutAdvancedSearchObj.ToDate) ? (DateTime?)null : DateTime.Parse(OutAdvancedSearchObj.ToDate);
                List<OutGoingChequesViewModel> OutGoingChequeObj = Mapper.Map<List<OutGoingCheques>, List<OutGoingChequesViewModel>>(_depositAndWithdrawalsBusiness.GetOutGoingCheques(outAdvancedSearchObj));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = OutGoingChequeObj });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion To List all Outgoingcheques

        #region To add outgoing cheques or edit it
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "R")]
        public string InsertUpdateOutgoingCheque(DepositAndWithdrwalViewModel depositAndWithdrawalObj)
        {
            try
            {
               
                AppUA appUA = Session["AppUA"] as AppUA;
                depositAndWithdrawalObj.OutGoingObj.CommonObj = new CommonViewModel();
                depositAndWithdrawalObj.OutGoingObj.CommonObj.CreatedBy = appUA.UserName;
                depositAndWithdrawalObj.OutGoingObj.CommonObj.CreatedDate = DateTime.Now;
                depositAndWithdrawalObj.OutGoingObj.CommonObj.UpdatedBy = appUA.UserName;
                depositAndWithdrawalObj.OutGoingObj.CommonObj.UpdatedDate = DateTime.Now;
                object OGC =_depositAndWithdrawalsBusiness.InsertUpdateOutgoingCheque(Mapper.Map<OutGoingChequesViewModel, OutGoingCheques>(depositAndWithdrawalObj.OutGoingObj));
                if (depositAndWithdrawalObj.OutGoingObj.ID != null && depositAndWithdrawalObj.OutGoingObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = OGC });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = OGC });
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion To add outgoing cheques or edit it

        #region To delete a record of outgoingcheque
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "D")]
        public string DeleteOutgoingCheque(DepositAndWithdrwalViewModel depositAndWithdrawalObj)
        {

            try
            {
                object result = null;
                result = _depositAndWithdrawalsBusiness.DeleteOutgoingCheque(depositAndWithdrawalObj.OutGoingObj.ID);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion To delete a record of outgoingcheque

        #region Get Details of outgoingcheques based on ID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "R")]
        public string GetOutgoingChequeById(string ID)
        {
            try
            {

                OutGoingChequesViewModel OutGoingChequeObj = Mapper.Map<OutGoingCheques, OutGoingChequesViewModel>(_depositAndWithdrawalsBusiness.GetOutgoingChequeById(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = OutGoingChequeObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion Get Details of outgoingcheques based on ID

        #region To Check Whether the same ChequeNo exists or not
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OutGoingCheques", Mode = "R")]
        public string ValidateChequeNo(OutGoingChequesViewModel OutGoingChequeObj)
        {
            object result = null;
            try

            {
                result = _depositAndWithdrawalsBusiness.ValidateChequeNo(Mapper.Map<OutGoingChequesViewModel, OutGoingCheques>(OutGoingChequeObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = "", Records = result });
            }

            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message, Status = -1 });
            }

        }
        #endregion To Check Whether the same ChequeNo exists or not


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "R")]
        public ActionResult IncomingCheques()
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            DateTime dateTime = appUA.DateTime;
            ViewBag.fromdate = dateTime.AddDays(-90).ToString("dd-MMM-yyyy");
            ViewBag.todate = dateTime.ToString("dd-MMM-yyyy");
            DepositAndWithdrwalViewModel depositAndWithdrwalViewModelObj = null;
            try
            {
                depositAndWithdrwalViewModelObj = new DepositAndWithdrwalViewModel();
                depositAndWithdrwalViewModelObj.bankList = new List<SelectListItem>();


                List<SelectListItem> selectListItem = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<BankViewModel> PayTermList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBanks());
                foreach (BankViewModel bvm in PayTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = bvm.Name,
                        Value = bvm.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.bankList = selectListItem;

                depositAndWithdrwalViewModelObj.CompanyList = new List<SelectListItem>();
                selectListItem = new List<SelectListItem>();
                List<CompaniesViewModel> CompaniesList = Mapper.Map<List<Companies>, List<CompaniesViewModel>>(_companiesBusiness.GetAllCompanies());
                foreach (CompaniesViewModel Cmp in CompaniesList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Cmp.Name,
                        Value = Cmp.Code,
                        Selected = false
                    });
                }
                depositAndWithdrwalViewModelObj.CompanyList = selectListItem;

                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomers());
                if (customerList != null)
                {
                    foreach (CustomerViewModel Cust in customerList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Text = Cust.CompanyName,
                            Value = Cust.ID.ToString(),
                            Selected = false
                        });
                    }
                }
                depositAndWithdrwalViewModelObj.customerList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(depositAndWithdrwalViewModelObj);
        }


        #region To List all IncomingCheques
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "R")]
        public string GetIncomingCheques(string incomingChequesAdvanceSearchObject)
        {
           
            try
            {
                OutgoingChequeAdvanceSearch outAdvancedSearchObj = incomingChequesAdvanceSearchObject != null ? JsonConvert.DeserializeObject<OutgoingChequeAdvanceSearch>(incomingChequesAdvanceSearchObject) : new OutgoingChequeAdvanceSearch();
                //DateTime? FDate = string.IsNullOrEmpty(OutAdvancedSearchObj.FromDate) ? (DateTime?)null : DateTime.Parse(OutAdvancedSearchObj.FromDate);
                //DateTime? TDate = string.IsNullOrEmpty(OutAdvancedSearchObj.ToDate) ? (DateTime?)null : DateTime.Parse(OutAdvancedSearchObj.ToDate);
                List<IncomingChequesViewModel> IncomingChequeObj = Mapper.Map<List<IncomingCheques>, List<IncomingChequesViewModel>>(_depositAndWithdrawalsBusiness.GetIncomingCheques(outAdvancedSearchObj));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = IncomingChequeObj });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion To List all IncomingCheques


        #region To add incoming cheques or edit it
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "R")]
        public string InsertUpdateIncomingCheque(DepositAndWithdrwalViewModel depositAndWithdrawalObj)
        {
            try
            {

                AppUA appUA = Session["AppUA"] as AppUA;
                depositAndWithdrawalObj.IncomingObj.CommonObj = new CommonViewModel();
                depositAndWithdrawalObj.IncomingObj.CommonObj.CreatedBy = appUA.UserName;
                depositAndWithdrawalObj.IncomingObj.CommonObj.CreatedDate = DateTime.Now;
                depositAndWithdrawalObj.IncomingObj.CommonObj.UpdatedBy = appUA.UserName;
                depositAndWithdrawalObj.IncomingObj.CommonObj.UpdatedDate = DateTime.Now;
                object OGC = _depositAndWithdrawalsBusiness.InsertUpdateIncomingCheque(Mapper.Map<IncomingChequesViewModel, IncomingCheques>(depositAndWithdrawalObj.IncomingObj));
                if (depositAndWithdrawalObj.IncomingObj.ID != null && depositAndWithdrawalObj.IncomingObj.ID != Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = OGC });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = OGC });
                }
            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion To add incoming cheques or edit it


        #region To delete a record of incomingcheque
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "D")]
        public string DeleteIncomingCheque(DepositAndWithdrwalViewModel depositAndWithdrawalObj)
        {

            try
            {
                object result = null;
                result = _depositAndWithdrawalsBusiness.DeleteIncomingCheque(depositAndWithdrawalObj.IncomingObj.ID);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion To delete a record of incomingcheque



        #region Get Details of incomingcheques based on ID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "R")]
        public string GetIncomingChequeById(string ID)
        {
            try
            {

                IncomingChequesViewModel IncomingChequeObj = Mapper.Map<IncomingCheques, IncomingChequesViewModel>(_depositAndWithdrawalsBusiness.GetIncomingChequeById(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = IncomingChequeObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion Get Details of incomingcheques based on ID


        #region To Check Whether the same ChequeNo exists or not
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "IncomingCheques", Mode = "R")]
        public string ValidateChequeNoIncomingCheque(IncomingChequesViewModel incomingChequeObj)
        {
            object result = null;
            try

            {
                result = _depositAndWithdrawalsBusiness.ValidateChequeNoIncomingCheque(Mapper.Map<IncomingChequesViewModel, IncomingCheques>(incomingChequeObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = "", Records = result });
            }

            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message, Status = -1 });
            }

        }
        #endregion To Check Whether the same ChequeNo exists or not
    }
}