using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class OtherExpenseBusiness: IOtherExpenseBusiness
    {
        IOtherExpenseRepository _otherExpenseRepository;
        IChartOfAccountsBusiness _chartOfAccountsBusiness;
        IBankBusiness _bankBusiness;
        ICompaniesBusiness _companiesBusiness;
        IPaymentModesBusiness _paymentModeBusiness;
        IEmployeeRepository _employeeRepository;
        ICommonBusiness _commonBusiness;
        public OtherExpenseBusiness(IOtherExpenseRepository otherExpenseRepository, IChartOfAccountsBusiness chartOfAccountsBusiness, IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness, IPaymentModesBusiness paymentModeBusiness, IEmployeeRepository employeeRepository, ICommonBusiness commonBusiness)
        {
            _otherExpenseRepository = otherExpenseRepository;
            _chartOfAccountsBusiness = chartOfAccountsBusiness;
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentModeBusiness = paymentModeBusiness;
            _employeeRepository = employeeRepository;
            _commonBusiness = commonBusiness;
        }

        public List<ChartOfAccounts> GetAllAccountTypes(string accountType)
        {
            List<ChartOfAccounts> accountsList = null;
            try
            {
                accountsList = _chartOfAccountsBusiness.GetChartOfAccountsByType(accountType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accountsList;
        }

        public List<Companies> GetAllCompanies()
        {
            List<Companies> companyList = null;
            try
            {
                companyList = _companiesBusiness.GetAllCompanies();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyList;
        }

        public List<PaymentModes> GetAllPaymentModes()
        {
            List<PaymentModes> paymentModeList = null;
            try
            {
                paymentModeList = _paymentModeBusiness.GetAllPaymentModes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paymentModeList;
        }

        public List<Bank> GetAllBankes()
        {
            List<Bank> bankList = null;
            try
            {
                bankList = _bankBusiness.GetAllBanks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bankList;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = null;
            try
            {
                employeeList = _employeeRepository.GetAllEmployees();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employeeList;
        }

        public List<OtherExpense> GetAllOtherExpenses()
        {
            List<OtherExpense> otherExpenseList = null;
            try
            {
                otherExpenseList = _otherExpenseRepository.GetAllOtherExpenses();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseList;
        }

        public OtherExpense InsertOtherExpense(OtherExpense otherExpense)
        {
            try
            {
                return  _otherExpenseRepository.InsertOtherExpense(otherExpense);
            }
            catch(Exception ex)
            {
                throw ex;
            }
         }

        public OtherExpense GetExpenseDetailsByID(Guid ID)
        {
            List<OtherExpense> otherExpenseList = null;
            OtherExpense otherExpense = null;
            try
            {
                otherExpenseList = GetAllOtherExpenses();
                otherExpense = otherExpenseList != null ? otherExpenseList.Where(o => o.ID == ID).ToList().FirstOrDefault() : null;
                if (otherExpense != null)
                {
                    otherExpense.creditAmountFormatted = _commonBusiness.ConvertCurrency(otherExpense.Amount, 2);
                  
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return otherExpense;
        }

        public OtherExpense UpdateOtherExpense(OtherExpense otherExpense)
        {
            return _otherExpenseRepository.UpdateOtherExpense(otherExpense);
        }

        public object DeleteOtherExpense(Guid ID, string UserName)
        {
            return _otherExpenseRepository.DeleteOtherExpense(ID,UserName);
        }

        public List<EmployeeType> GetAllEmployeeTypes()
        {
            return _employeeRepository.GetAllEmployeeTypes();
        }

        public List<Employee> GetAllEmployeesByType(string Type)
        {
            List<Employee> empList = null;
            try
            {
                empList=GetAllEmployees();
                empList = empList != null ? empList.Where(e => e.employeeTypeObj.Code == Type).ToList() : null;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return empList;
        }

        public OtherExpSummary GetOtherExpSummary(int month, int year, string Company) {

            OtherExpSummary result= _otherExpenseRepository.GetOtherExpSummary(month, year, Company);



            if (result != null)
            {
                int r = 150;
                int g = 120;
                int b = 80;
                string color = "rgba($r$,$g$,$b$,0.6)";
                foreach (OtherExpSummaryItem s in result.ItemsList)
                {
                    Random rnd = new Random();
                  
                    s.color = color.Replace("$r$", r.ToString()).Replace("$g$", g.ToString()).Replace("$b$", b.ToString());
                    b = b + 50;
                    g = g + 30;
                    r = r + g;
                    if (b > 250)
                    {
                        b = 0;
                    }
                    if (g > 250)
                    {
                        g = 0;

                    }
                    if (r > 250)
                    {
                        r = 0;
                    }


                    s.AmountFormatted = _commonBusiness.ConvertCurrency(s.Amount, 2);
                    result.Total = result.Total +s.Amount;
                }

                result.TotalFormatted = _commonBusiness.ConvertCurrency(result.Total, 2);
                result.Month = new DateTime(2010, month, 1).ToString("MMM") + " " + year;
            }
            return result;
        }

        public List<OtherExpense> GetExpenseTypeDetails(OtherExpense expObj)
        {
            return _otherExpenseRepository.GetExpenseTypeDetails(expObj);
        }

        public List<Employee> GetCompanybyEmployee(Guid ID)
        {
            List<Employee> empList = null;
            try
            {
                empList = GetAllEmployees();
                empList = empList != null ? empList.Where(e => e.ID==ID).ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return empList;

        }

        public OtherExpense GetOpeningBalance(string OpeningDate)
        {
            try
            {
                OtherExpense OtherExpenseObj = null;

                OtherExpenseObj= _otherExpenseRepository.GetOpeningBalance(OpeningDate);
                OtherExpenseObj.OpeningBank = _commonBusiness.ConvertCurrency(decimal.Parse(OtherExpenseObj.OpeningBank));
                OtherExpenseObj.OpeningCash = _commonBusiness.ConvertCurrency(decimal.Parse(OtherExpenseObj.OpeningCash));
                OtherExpenseObj.OpeningNCBank = _commonBusiness.ConvertCurrency(decimal.Parse(OtherExpenseObj.OpeningNCBank));
                OtherExpenseObj.UndepositedCheque = _commonBusiness.ConvertCurrency(decimal.Parse(OtherExpenseObj.UndepositedCheque));
                return OtherExpenseObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OtherExpense> GetBankWiseBalance(String Date)
        {
            List<OtherExpense> otherExpenseList = null;
            try
            {
                otherExpenseList = _otherExpenseRepository.GetBankWiseBalance(Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpenseList;
        }
    }
}