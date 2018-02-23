using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class ReportBusiness : IReportBusiness
    {
        IReportRepository _reportRepository;
        ICommonBusiness _commonBusiness;
        public ReportBusiness(IReportRepository reportRepository, ICommonBusiness commonBusiness)
        {
            _reportRepository = reportRepository;
            _commonBusiness = commonBusiness;
        }

        public List<SystemReport> GetAllSysReports(AppUA appUA)
        {
            List<SystemReport> systemReportList = null;
            try
            {
                systemReportList = _reportRepository.GetAllSysReports(appUA);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return systemReportList;
        }

        public List<CustomerContactDetailsReport> GetCustomerContactDetailsReport(string search)
        {
            List<CustomerContactDetailsReport> CustomerContactDetailsList = null;
            try
            {
                CustomerContactDetailsList = _reportRepository.GetCustomerContactDetailsReport(search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerContactDetailsList;
        }

        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search,string ExpenseType)
        {
            List<OtherExpenseDetailsReport> otherExpenseDetailsList = null;
            try
            {
                otherExpenseDetailsList = _reportRepository.GetOtherExpenseDetails(FromDate, ToDate, CompanyCode, accounthead,  subtype, employeeorother, employeecompany, search, ExpenseType);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseDetailsList;
        }

        public List<OtherExpenseDetailsReport> GetOtherExpenseLimitedDetailReport(OtherExpenseLimitedExpenseAdvanceSearch limitedExpenseAdvanceSearchObject)
        {
            List<OtherExpenseDetailsReport> otherExpenseLimitedList = null;
            try
            {
                otherExpenseLimitedList = _reportRepository.GetOtherExpenseLimitedDetailReport(limitedExpenseAdvanceSearchObject);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return otherExpenseLimitedList;
        }


        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy, string accounthead, string subtype, string employeeorother,string employeecompany, string search, string ExpenseType)
        {
            List<OtherExpenseSummaryReport> otherExpenseSummaryList = null;
           
            try
            {
                otherExpenseSummaryList = _reportRepository.GetOtherExpenseSummary(FromDate, ToDate, CompanyCode,ReportType, accounthead, subtype, employeeorother, employeecompany, search, ExpenseType);
                
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseSummaryList;
        }

        public PurchaseDetailReport GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Guid Supplier,string InvoiceType,Guid SubType, string AccountCode)
        {
            PurchaseDetailReport purchasedetailObj = new PurchaseDetailReport();
            List<PurchaseDetailReport> purchaseDetailReportList = null;
            try
            {
                purchaseDetailReportList = _reportRepository.GetPurchaseDetails(FromDate, ToDate, CompanyCode,search,IsInternal,Supplier, InvoiceType, SubType, AccountCode);
                decimal purchaseDetailSum = purchaseDetailReportList.Where(PD => PD.RowType != "T").Sum(PD => PD.BalanceDue);
                decimal purchaseDetailInvoiceAmount = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.InvoiceAmount);
                decimal purchaseDetailPaidAmount = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.PaidAmount);
                decimal purchaseDetailPaymentProcessed = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.PaymentProcessed);
                decimal purchaseDetailsTax = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.Tax);
                decimal purchaseDetailTotal = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.TotalInvoice);
                purchasedetailObj.purchaseDetailSum = _commonBusiness.ConvertCurrency(purchaseDetailSum, 2);
                purchasedetailObj.purchaseDetailPaid = _commonBusiness.ConvertCurrency(purchaseDetailPaidAmount, 2);
                purchasedetailObj.purchaseDetailInvoice = _commonBusiness.ConvertCurrency(purchaseDetailInvoiceAmount, 2);
                purchasedetailObj.purchaseDetailPaymentProcess= _commonBusiness.ConvertCurrency(purchaseDetailPaymentProcessed, 2);
                purchasedetailObj.purchaseDetailsTaxAmount = _commonBusiness.ConvertCurrency(purchaseDetailsTax, 2);
                purchasedetailObj.purchaseDetailsTotalAmount = _commonBusiness.ConvertCurrency(purchaseDetailTotal, 2);
                purchasedetailObj.purchaseDetailReportList = purchaseDetailReportList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchasedetailObj;
        }

        public PurchaseSummaryReport GetPurchaseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal)
        {
            PurchaseSummaryReport PurchaseObj = new PurchaseSummaryReport();
            List<PurchaseSummaryReport> purchaseSummaryReportList = null;
            try
            {
                purchaseSummaryReportList = _reportRepository.GetPurchaseSummary(FromDate, ToDate, CompanyCode,search, IsInternal);
                decimal purchaseSummarySum = purchaseSummaryReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.NetDue);
                decimal purchaseSummaryInvoiceAmount = purchaseSummaryReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.Invoiced);
                decimal purchaseSummaryPaidAmount = purchaseSummaryReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.Paid);
                PurchaseObj.purchaseSummarySum = _commonBusiness.ConvertCurrency(purchaseSummarySum, 2);
                PurchaseObj.purchaseSummaryPaid = _commonBusiness.ConvertCurrency(purchaseSummaryPaidAmount, 2);
                PurchaseObj.purchaseSummaryInvoice = _commonBusiness.ConvertCurrency(purchaseSummaryInvoiceAmount, 2);
                PurchaseObj.purchaseSummaryReportList = purchaseSummaryReportList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseObj;
        }

        public PurchaseDetailReport GetRPTViewPurchaseDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, Guid SupplierID)
        {
            PurchaseDetailReport detailObj = new PurchaseDetailReport();
            List<PurchaseDetailReport> DetailList = null;
            try
            {
                DetailList = _reportRepository.GetRPTViewPurchaseDetail(FromDate, ToDate, CompanyCode, SupplierID);
                detailObj.purchaseDetailReportList = DetailList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return detailObj;
        }

        public List<PurchaseTransactionLogReport> GetPurchaseTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<PurchaseTransactionLogReport> purchaseTransactionLogReportList = null;
            try
            {
                purchaseTransactionLogReportList = _reportRepository.GetPurchaseTransactionLogDetails(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseTransactionLogReportList;
        }

        public SaleDetailReport GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search,Boolean IsInternal,Boolean IsTax, Guid Customer,string InvoiceType)
        {
            SaleDetailReport SaledetailObj = new SaleDetailReport();
            List<SaleDetailReport> saleDetailList = null;
            try
            {
                
                saleDetailList = _reportRepository.GetSaleDetail(FromDate,ToDate, CompanyCode,search,IsInternal,IsTax,Customer, InvoiceType);
                decimal saledetailsum = saleDetailList.Where(SD => SD.RowType != "T").Sum(SD => SD.BalanceDue);
                decimal saledetailinvoiceamount = saleDetailList.Where(SD => SD.RowType != "T").Sum(SD => SD.InvoiceAmount);
                decimal saledetailpaidamount = saleDetailList.Where(SD => SD.RowType != "T").Sum(SD => SD.PaidAmount);
                decimal saledetailTax = saleDetailList.Where(SS => SS.RowType != "T").Sum(SD => SD.TaxAmount);
                decimal saledetailTotalInvoiced= saleDetailList.Where(SS => SS.RowType != "T").Sum(SD => SD.Total);

                SaledetailObj.saledetailSum = _commonBusiness.ConvertCurrency(saledetailsum, 2);
                SaledetailObj.saledetailinvoice = _commonBusiness.ConvertCurrency(saledetailinvoiceamount, 2);
                SaledetailObj.saledetailpaid = _commonBusiness.ConvertCurrency(saledetailpaidamount, 2);
                SaledetailObj.saledetailtax = _commonBusiness.ConvertCurrency(saledetailTax, 2);
                SaledetailObj.saledetailtotalinvoiced= _commonBusiness.ConvertCurrency(saledetailTotalInvoiced, 2);
                SaledetailObj.saleDetailList = saleDetailList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SaledetailObj;
        }

        public SaleDetailReport GetRPTViewCustomerDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode,Guid Customer)
        {
            SaleDetailReport SaledetailObj = new SaleDetailReport();
            List<SaleDetailReport> saleDetailList = null;
            try
            {
                saleDetailList = _reportRepository.GetRPTViewCustomerDetail(FromDate, ToDate, CompanyCode,Customer);
                SaledetailObj.saleDetailList = saleDetailList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SaledetailObj;
        }

        public List<SalesTransactionLogReport> GetSalesTransactionLogDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search)
        {
            List<SalesTransactionLogReport> salesTransactionLogReportList = null;
            try
            {
                salesTransactionLogReportList = _reportRepository.GetSalesTransactionLogDetails(FromDate, ToDate, CompanyCode,search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return salesTransactionLogReportList;
        }

        public SaleSummary GetSaleSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Boolean IsTax)
        {
            SaleSummary saleObj = new SaleSummary();
            List<SaleSummary> saleSummaryList = null;
            try
            {
                saleSummaryList = _reportRepository.GetSaleSummary(FromDate, ToDate, CompanyCode,search,IsInternal,IsTax);
                decimal salesummarySum = saleSummaryList.Where(SS => SS.RowType != "T").Sum(SS => SS.NetDue);
                decimal salesummaryTax = saleSummaryList.Where(SS => SS.RowType != "T").Sum(SS => SS.TaxAmount);
                decimal salesummaryInvoice = saleSummaryList.Where(SS => SS.RowType != "T").Sum(SS => SS.Invoiced);
                decimal salesummaryinvoicetotal = saleSummaryList.Where(SS => SS.RowType != "T").Sum(SS => SS.Total);
                decimal salesummarypaid = saleSummaryList.Where(SS => SS.RowType != "T").Sum(SS => SS.Paid);


                saleObj.salesummarySum = _commonBusiness.ConvertCurrency(salesummarySum, 2);
                saleObj.salesummaryInvoiced = _commonBusiness.ConvertCurrency(salesummaryInvoice, 2);
                saleObj.salesummaryTotalInvoice = _commonBusiness.ConvertCurrency(salesummaryinvoicetotal, 2);
                saleObj.salesummarypaid = _commonBusiness.ConvertCurrency(salesummarypaid, 2);
                saleObj.salesummaryTax = _commonBusiness.ConvertCurrency(salesummaryTax, 2);
                saleObj.saleSummaryList = saleSummaryList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return saleObj;
        }

        public List<SupplierContactDetailsReport> GetSupplierContactDetailsReport(string search)
        {
            List<SupplierContactDetailsReport> supplierContactDetailsReportList = null;
            try
            {
                supplierContactDetailsReportList = _reportRepository.GetSupplierContactDetailsReport(search);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supplierContactDetailsReportList;
        }

        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(ReportAccountsReceivableAgeingSearch AccountsReceivableAgeingSearchObj)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(AccountsReceivableAgeingSearchObj);
                if(accountsReceivableAgeingReportList!=null)
                accountsReceivableAgeingReportList=accountsReceivableAgeingReportList.Where(C => C.InvoiceType == "RB").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingReportList;
        }

        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(ReportAccountsReceivableAgeingSearch AccountsReceivableAgeingSearchObj)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(AccountsReceivableAgeingSearchObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingReportList;
        }

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
            try
            {
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReport(FromDate, ToDate, CompanyCode, Customerids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingSummaryReportList;
        }

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string Customerids,string InvoiceType)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
            try
            {
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReportForSA(FromDate, ToDate, CompanyCode, Customerids, InvoiceType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingSummaryReportList;
        }

        public List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(ReportAdvanceSearch reportAdvanceSearchObj)
        {
            List<AccountsPayableAgeingReport> accountsPayableAgeingReportList = null;
            try
            {
                accountsPayableAgeingReportList = _reportRepository.GetAccountsPayableAgeingReport(reportAdvanceSearchObj); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsPayableAgeingReportList;
        }

        public List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(ReportAdvanceSearch reportAdvanceSearchObj)
        {
            List<AccountsPayableAgeingSummaryReport> accountsPayableAgeingSummaryReportList = null;
            try
            {
                accountsPayableAgeingSummaryReportList = _reportRepository.GetAccountsPayableAgeingSummaryReport(reportAdvanceSearchObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsPayableAgeingSummaryReportList;
        }

        public List<EmployeeExpenseSummaryReport> GetEmployeeExpenseSummary(DateTime? FromDate, DateTime? ToDate, string EmployeeCode, string OrderBy)
        {
            List<EmployeeExpenseSummaryReport> employeeExpenseSummaryList = null;
            try
            {
                employeeExpenseSummaryList = _reportRepository.GetEmployeeExpenseSummary(FromDate, ToDate, EmployeeCode);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeExpenseSummaryList;
        }

        public List<DepositsAndWithdrawalsDetailsReport> GetDepositAndWithdrawalDetail(DateTime? FromDate, DateTime? ToDate, string BankCode, string search)
        {
            List<DepositsAndWithdrawalsDetailsReport> depositAndWithdrawalDetailList = null;
            try
            {
                depositAndWithdrawalDetailList = _reportRepository.GetDepositAndWithdrawalDetail(FromDate, ToDate, BankCode, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return depositAndWithdrawalDetailList;
        }

        public List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search)
        {
            List<OtherIncomeSummaryReport> otherIncomeSummaryList = null;

            try
            {
                otherIncomeSummaryList = _reportRepository.GetOtherIncomeSummary(FromDate, ToDate, CompanyCode, accounthead, subtype, employeeorother, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeSummaryList;
        }

        public List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string subtype, string employeeorother, string search)
        {
            List<OtherIncomeDetailsReport> otherIncomeDetailsList = null;
            try
            {
                otherIncomeDetailsList = _reportRepository.GetOtherIncomeDetails(FromDate, ToDate, CompanyCode, accounthead, subtype,employeeorother, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeDetailsList;
        }

        public List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? fromDate, DateTime? toDate, string customerIDs, string company,string invoiceType)
        {
            List<CustomerPaymentLedger> CustomerPaymentsDetailsList = null;
            try
            {
                CustomerPaymentsDetailsList = _reportRepository.GetCustomerPaymentLedger(fromDate,toDate, customerIDs, company,invoiceType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerPaymentsDetailsList;
        }

        public List<SupplierPaymentLedger> GetSupplierPaymentLedger(DateTime? fromDate, DateTime? toDate, string supplierCode, string company,string invoiceType)
        {
            List<SupplierPaymentLedger> SupplierPaymentsDetailsList = null;
            try
            {
                SupplierPaymentsDetailsList = _reportRepository.GetSupplierPaymentLedger(fromDate, toDate, supplierCode,company,invoiceType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SupplierPaymentsDetailsList;
        }

        public List<DailyLedgerReport> GetDailyLedgerDetails(DateTime? FromDate, DateTime? ToDate, DateTime? Date, string MainHead, string search,string Bank)
        {
            List<DailyLedgerReport> dailyLedgerList = null;
            try
            {
                dailyLedgerList = _reportRepository.GetDailyLedgerDetails(FromDate, ToDate, Date, MainHead, search,Bank);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dailyLedgerList;
        }

        public List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate,String Filter,string Company,string Customer,string InvoiceType)
        {
            List<CustomerExpeditingReport> customerExpeditingList = null;
            try
            {
                customerExpeditingList = _reportRepository.GetCustomerExpeditingDetail(ToDate, Filter,Company,Customer, InvoiceType);                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerExpeditingList;
        }
     
        public List<SupplierExpeditingReport> GetSupplierExpeditingDetail(ReportAdvanceSearch advanceSearchObject)//(DateTime? ToDate, string Filter,string Company,string Supplier)
        {
            List<SupplierExpeditingReport> supplierExpeditingList = null;
            try
            {
                supplierExpeditingList = _reportRepository.GetSupplierExpeditingDetail(advanceSearchObject);// (ToDate,Filter,Company,Supplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supplierExpeditingList;
        }

        public List<FollowupReport> GetFollowupReportDetail(FollowupReportAdvanceSearch advanceSearchObject)
        {
            List<FollowupReport> followupList = null;
            try
            {
                followupList = _reportRepository.GetFollowupReport(advanceSearchObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return followupList;
        }

        public List<TrialBalance> GetTrialBalanceReport(DateTime? Date)
        {
            List<TrialBalance> TBlist = null;
            try
            {
                TBlist = _reportRepository.GetTrialBalanceReport(Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TBlist;
        }
    }
}
