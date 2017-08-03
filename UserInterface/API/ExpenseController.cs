using AutoMapper;
using Newtonsoft.Json;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserInterface.Models;

namespace UserInterface.API
{
    public class ExpenseController : ApiController
    {
        #region Constructor_Injection

        IOtherExpenseBusiness _otherExpenseBusiness;


        public ExpenseController(IOtherExpenseBusiness otherExpenseBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness ;

        }
        #endregion Constructor_Injection

        Const messages = new Const();

        #region GetExpenseDetailsByValue
        [HttpPost]
        public string GetExpenseDetailsForMobile(OtherExpense expObj)
        {
            try
            {
                List<OtherExpenseViewModel> expenseObj = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetExpenseTypeDetails(expObj));
                 
                return JsonConvert.SerializeObject(new { Result = true, Records = expenseObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion  GetExpenseDetailsByValue
    }
}
