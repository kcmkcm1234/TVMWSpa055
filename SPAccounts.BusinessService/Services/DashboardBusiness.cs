using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class DashboardBusiness:IDashboardBusiness
    {
        private IDashboardRepository _dashboardRepository;
        private ICommonBusiness _commonBusiness;
        public DashboardBusiness(IDashboardRepository dashboardRepository, ICommonBusiness commonBusiness)
        {
            _dashboardRepository = dashboardRepository;
            _commonBusiness = commonBusiness;
        }
        public MonthlyRecap GetMonthlyRecap(string Company)
        {
            MonthlyRecap Result= _dashboardRepository.GetMonthlyRecap(Company);
            if (Result != null) {
                foreach (MonthlyRecapItem m in Result.MonthlyRecapItemList) {
                    Result.TotalIncome = Result.TotalIncome + m.INAmount;
                    Result.TotalExpense = Result.TotalExpense + m.ExAmount;
                }

                Result.TotalProfit = Result.TotalIncome - Result.TotalExpense;
                //decimal n = (Result.MonthlyRecapItemList[11].INAmount - Result.MonthlyRecapItemList[0].INAmount);
                //decimal d = Result.MonthlyRecapItemList[0].INAmount;
                //if (d == 0) { d = 1; }
                //Result.IncomePercentage = n /d * 100;

                //n = (Result.MonthlyRecapItemList[11].ExAmount - Result.MonthlyRecapItemList[0].ExAmount);
                //d = Result.MonthlyRecapItemList[0].ExAmount;
                //if (d == 0) { d = 1; }
                //Result.ExpensePercentage = n/ d * 100;

                //n = ((Result.MonthlyRecapItemList[11].INAmount - Result.MonthlyRecapItemList[11].ExAmount) - (Result.MonthlyRecapItemList[0].INAmount - Result.MonthlyRecapItemList[0].ExAmount));
                //d = (Result.MonthlyRecapItemList[0].INAmount - Result.MonthlyRecapItemList[0].ExAmount);
                //if (d == 0) { d = 1; }

                //Result.ProfitPercentage =  n/d  * 100;
                Result.Caption = Result.MonthlyRecapItemList[0].Period + "-" + Result.MonthlyRecapItemList[11].Period;

                Result.FormattedTotalIncome = _commonBusiness.ConvertCurrency(Result.TotalIncome, 2);
                Result.FormattedTotalExpense = _commonBusiness.ConvertCurrency(Result.TotalExpense, 2);
                Result.FormattedTotalProfit = _commonBusiness.ConvertCurrency(Result.TotalProfit, 2);

            }

            return Result;

        }

    }
}