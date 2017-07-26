﻿using SPAccounts.BusinessService.Contracts;
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
        public OtherExpenseBusiness(IOtherExpenseRepository otherExpenseRepository, IChartOfAccountsBusiness chartOfAccountsBusiness, IBankBusiness bankBusiness, ICompaniesBusiness companiesBusiness, IPaymentModesBusiness paymentModeBusiness, IEmployeeRepository employeeRepository)
        {
            _otherExpenseRepository = otherExpenseRepository;
            _chartOfAccountsBusiness = chartOfAccountsBusiness;
            _bankBusiness = bankBusiness;
            _companiesBusiness = companiesBusiness;
            _paymentModeBusiness = paymentModeBusiness;
            _employeeRepository = employeeRepository;
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
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return otherExpense;
        }

        public OtherExpense UpdateOtherExpense(OtherExpense otherExpense)
        {
            throw new NotImplementedException();
        }

        public object DeleteOtherExpense(OtherExpense otherExpense)
        {
            throw new NotImplementedException();
        }
    }
}