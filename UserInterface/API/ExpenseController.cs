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

        IChartOfAccountsBusiness _chartofaccountsBusiness;


        public ExpenseController(IChartOfAccountsBusiness chartofaccountsBusiness)
        {
            _chartofaccountsBusiness = chartofaccountsBusiness;

        }
        #endregion Constructor_Injection

        Const messages = new Const();

        #region GetExpenseDetailsByValue
        [HttpPost]
        public string GetExpenseDetailsForMobile(ChartOfAccounts account)
        {
            try
            {
                List<ChartOfAccountsViewModel> expenseObj = Mapper.Map<List<ChartOfAccounts>, List<ChartOfAccountsViewModel>>(_chartofaccountsBusiness.GetExpenseTypeDetails(account));
                 
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
