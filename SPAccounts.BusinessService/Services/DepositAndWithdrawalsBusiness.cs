﻿using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class DepositAndWithdrawalsBusiness : IDepositAndWithdrawalsBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IDepositAndWithdrawalsRepository _depositAndWithdrawalsRepository;

        public DepositAndWithdrawalsBusiness(IDepositAndWithdrawalsRepository iDepositAndWithdrawalsRepository, ICommonBusiness commonBusiness)
        {
            _depositAndWithdrawalsRepository = iDepositAndWithdrawalsRepository;
            _commonBusiness = commonBusiness;
        }
        public List<DepositAndWithdrawals> GetAllDepositAndWithdrawals(string FromDate, string ToDate, string DepositOrWithdrawal,string chqclr)
        {
            return _depositAndWithdrawalsRepository.GetAllDepositAndWithdrawals(FromDate, ToDate,DepositOrWithdrawal, chqclr);
        }

        public DepositAndWithdrawals GetDepositAndWithdrawalDetails(Guid ID)
        {
            DepositAndWithdrawals depositAndWithdrawalsObj = new DepositAndWithdrawals();
            depositAndWithdrawalsObj = _depositAndWithdrawalsRepository.GetDepositAndWithdrawalDetails(ID);
            if (depositAndWithdrawalsObj != null)
            {
                depositAndWithdrawalsObj.AmountFormatted = _commonBusiness.ConvertCurrency(depositAndWithdrawalsObj.Amount, 2);

            }
            return depositAndWithdrawalsObj;
        }
        public object InsertUpdateDepositAndWithdrawals(DepositAndWithdrawals _depositAndWithdrawalsObj)
        {
            object result = null;
            try
            {
                if (_depositAndWithdrawalsObj.CheckedRows.Count>0)
                {
                    for (int i = 0; i < _depositAndWithdrawalsObj.CheckedRows.Count; i++)
                    {
                       
                       
                        result = _depositAndWithdrawalsRepository.InsertDepositAndWithdrawals(_depositAndWithdrawalsObj.CheckedRows[i]);
                    }

                }
                else
                {
                    if (_depositAndWithdrawalsObj.ID == Guid.Empty)
                    {
                        result = _depositAndWithdrawalsRepository.InsertDepositAndWithdrawals(_depositAndWithdrawalsObj);
                    }
                    else
                    {
                        result = _depositAndWithdrawalsRepository.UpdateDepositAndWithdrawals(_depositAndWithdrawalsObj);
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public object ClearCheque(List<Guid> data)
        {
            object result = null;
            try
            {
                
                int len = data.Count;
                
                for(int i=0;i<len;i++)
                {                    
                    result = _depositAndWithdrawalsRepository.ClearCheque(data[i]);
                }
                    
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}