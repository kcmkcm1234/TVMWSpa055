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

        public List<OtherExpenseDetailsReport> GetOtherExpenseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string OrderBy, string accounthead, string subtype, string employeeorother, string employeecompany, string search)
        {
            List<OtherExpenseDetailsReport> otherExpenseDetailsList = null;
            try
            {
                otherExpenseDetailsList = _reportRepository.GetOtherExpenseDetails(FromDate, ToDate, CompanyCode, accounthead,  subtype, employeeorother, employeecompany, search);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseDetailsList;
        }

        public List<OtherExpenseSummaryReport> GetOtherExpenseSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string ReportType, string OrderBy, string accounthead, string subtype, string employeeorother,string employeecompany, string search)
        {
            List<OtherExpenseSummaryReport> otherExpenseSummaryList = null;
           
            try
            {
                otherExpenseSummaryList = _reportRepository.GetOtherExpenseSummary(FromDate, ToDate, CompanyCode,ReportType, accounthead, subtype, employeeorother, employeecompany, search);
                
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseSummaryList;
        }

        public PurchaseDetailReport GetPurchaseDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search, Boolean IsInternal, Guid Supplier)
        {
            PurchaseDetailReport purchasedetailObj = new PurchaseDetailReport();
            List<PurchaseDetailReport> purchaseDetailReportList = null;
            try
            {
                purchaseDetailReportList = _reportRepository.GetPurchaseDetails(FromDate, ToDate, CompanyCode,search,IsInternal,Supplier);
                decimal purchaseDetailSum = purchaseDetailReportList.Where(PD => PD.RowType != "T").Sum(PD => PD.BalanceDue);
                decimal purchaseDetailInvoiceAmount = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.InvoiceAmount);
                decimal purchaseDetailPaidAmount = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.PaidAmount);
                decimal purchaseDetailPaymentProcessed = purchaseDetailReportList.Where(PS => PS.RowType != "T").Sum(PS => PS.PaymentProcessed);
                purchasedetailObj.purchaseDetailSum = _commonBusiness.ConvertCurrency(purchaseDetailSum, 2);
                purchasedetailObj.purchaseDetailPaid = _commonBusiness.ConvertCurrency(purchaseDetailPaidAmount, 2);
                purchasedetailObj.purchaseDetailInvoice = _commonBusiness.ConvertCurrency(purchaseDetailInvoiceAmount, 2);
                purchasedetailObj.purchaseDetailPaymentProcess= _commonBusiness.ConvertCurrency(purchaseDetailPaymentProcessed, 2);
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

        public SaleDetailReport GetSaleDetail(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string search,Boolean IsInternal,Boolean IsTax, Guid Customer)
        {
            SaleDetailReport SaledetailObj = new SaleDetailReport();
            List<SaleDetailReport> saleDetailList = null;
            try
            {
                
                saleDetailList = _reportRepository.GetSaleDetail(FromDate,ToDate, CompanyCode,search,IsInternal,IsTax,Customer);
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
        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(FromDate, ToDate, CompanyCode);
                accountsReceivableAgeingReportList=accountsReceivableAgeingReportList.Where(C => C.InvoiceType == "RB").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingReportList;
        }
        public List<AccountsReceivableAgeingReport> GetAccountsReceivableAgeingReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingReport> accountsReceivableAgeingReportList = null;
            try
            {
                accountsReceivableAgeingReportList = _reportRepository.GetAccountsReceivableAgeingReport(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingReportList;
        }

        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
            try
            {
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReport(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingSummaryReportList;
        }
        public List<AccountsReceivableAgeingSummaryReport> GetAccountsReceivableAgeingSummaryReportForSA(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsReceivableAgeingSummaryReport> accountsReceivableAgeingSummaryReportList = null;
            try
            {
                accountsReceivableAgeingSummaryReportList = _reportRepository.GetAccountsReceivableAgeingSummaryReportForSA(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsReceivableAgeingSummaryReportList;
        }

        public List<AccountsPayableAgeingReport> GetAccountsPayableAgeingReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsPayableAgeingReport> accountsPayableAgeingReportList = null;
            try
            {
                accountsPayableAgeingReportList = _reportRepository.GetAccountsPayableAgeingReport(FromDate, ToDate, CompanyCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsPayableAgeingReportList;
        }

        public List<AccountsPayableAgeingSummaryReport> GetAccountsPayableAgeingSummaryReport(DateTime? FromDate, DateTime? ToDate, string CompanyCode)
        {
            List<AccountsPayableAgeingSummaryReport> accountsPayableAgeingSummaryReportList = null;
            try
            {
                accountsPayableAgeingSummaryReportList = _reportRepository.GetAccountsPayableAgeingSummaryReport(FromDate, ToDate, CompanyCode);
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
        public List<OtherIncomeSummaryReport> GetOtherIncomeSummary(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)
        {
            List<OtherIncomeSummaryReport> otherIncomeSummaryList = null;

            try
            {
                otherIncomeSummaryList = _reportRepository.GetOtherIncomeSummary(FromDate, ToDate, CompanyCode, accounthead, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeSummaryList;
        }

        public List<OtherIncomeDetailsReport> GetOtherIncomeDetails(DateTime? FromDate, DateTime? ToDate, string CompanyCode, string accounthead, string search)
        {
            List<OtherIncomeDetailsReport> otherIncomeDetailsList = null;
            try
            {
                otherIncomeDetailsList = _reportRepository.GetOtherIncomeDetails(FromDate, ToDate, CompanyCode, accounthead, search);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherIncomeDetailsList;
        }

         public List<CustomerPaymentLedger> GetCustomerPaymentLedger(DateTime? FromDate, DateTime? ToDate, string CustomerIDs, string Company)
        {
            List<CustomerPaymentLedger> CustomerPaymentsDetailsList = null;
            try
            {
                CustomerPaymentsDetailsList = _reportRepository.GetCustomerPaymentLedger(FromDate,ToDate, CustomerIDs, Company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerPaymentsDetailsList;
        }
        public List<SupplierPaymentLedger> GetSupplierPaymentLedger(DateTime? FromDate, DateTime? ToDate, string Suppliercode, string Company)
        {
            List<SupplierPaymentLedger> SupplierPaymentsDetailsList = null;
            try
            {
                SupplierPaymentsDetailsList = _reportRepository.GetSupplierPaymentLedger(FromDate, ToDate, Suppliercode,Company);
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


        public List<CustomerExpeditingReport> GetCustomerExpeditingDetail(DateTime? ToDate)
        {
            List<CustomerExpeditingReport> CustomerExpeditingList = null;
            try
            {
                CustomerExpeditingList = _reportRepository.GetCustomerExpeditingDetail(ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerExpeditingList;
        }


        

            public List<SupplierExpeditingReport> GetSupplierExpeditingDetail(DateTime? ToDate)
        {
            List<SupplierExpeditingReport> SupplierExpeditingList = null;
            try
            {
                SupplierExpeditingList = _reportRepository.GetSupplierExpeditingDetail(ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SupplierExpeditingList;
        }
    }
}
