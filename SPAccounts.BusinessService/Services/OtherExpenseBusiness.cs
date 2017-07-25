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
        public OtherExpenseBusiness(IOtherExpenseRepository otherExpenseRepository)
        {
            _otherExpenseRepository = otherExpenseRepository;
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
    }
}