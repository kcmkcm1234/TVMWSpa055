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
                if (expObj == null) throw new Exception(messages.NoItems);

                List<OtherExpenseViewModel> expenseObj = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetExpenseTypeDetails(expObj));
                 
                return JsonConvert.SerializeObject(new { Result = true, Records = expenseObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion  GetExpenseDetailsByValue

        #region GetOtherExpenseByID
        [HttpPost]
        public string GetOtherExpenseByIDForMobile(OtherExpense otherExpenseObj)
        {
            try
            {
                if (otherExpenseObj.ID != Guid.Empty && otherExpenseObj.ID != null)
                {
                    OtherExpenseViewModel expenseObj = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.GetOtherExpenseByID(otherExpenseObj.ID));
                    return JsonConvert.SerializeObject(new { Result = true, Records = expenseObj });
                }
                else
                {
                    throw new Exception("Invalid Input");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetOtherExpenseByID

        #region GetAllPendingForApprovalExpense
        [HttpGet]
        public string GetAllPendingForApprovalExpenseForMobile()
        {
            try
            {
                List<OtherExpenseViewModel> expenseObj = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetAllOtherExpenseByApprovalStatus(1,null));
                return JsonConvert.SerializeObject(new { Result = true, Records = expenseObj });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion GetAllPendingForApprovalExpense

        #region ApproveOtherExpense
        [HttpPost]
        public string ApproveOtherExpenseForMobile(OtherExpense otherExpenseObj)
        {
            try
            {
                if (otherExpenseObj.ID != Guid.Empty && otherExpenseObj.ID != null)
                {
                    string resultMessage = _otherExpenseBusiness.ApproveOtherExpense(otherExpenseObj.ID);
                    return JsonConvert.SerializeObject(new { Result = true, Message = resultMessage });
                }
                else
                {
                    throw new Exception("Invalid Input");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion ApproveOtherExpense
    }
}
