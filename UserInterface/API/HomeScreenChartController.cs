using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;

using UserInterface.Models;
using Newtonsoft.Json;

namespace UserInterface.API
{
    public class HomeScreenChartController : ApiController
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        IDashboardBusiness _dashboardBusiness;


        public HomeScreenChartController(IDashboardBusiness dashboardBusiness)
        {
            _dashboardBusiness = dashboardBusiness;

        }
        #endregion Constructor_Injection 

        [HttpPost]
        public string SalesSummaryChartForMobile(SalesSummary duration)
        {
            try
            {
                List<SalesSummaryViewModel> saleObj = Mapper.Map<List<SalesSummary>, List<SalesSummaryViewModel>>(_dashboardBusiness.GetSalesSummaryChart(duration));

                return JsonConvert.SerializeObject(new { Result = true, Records = saleObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
     
    }
}
