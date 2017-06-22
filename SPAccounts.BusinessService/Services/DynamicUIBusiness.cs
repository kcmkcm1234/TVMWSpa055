using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
 
namespace SPAccounts.BusinessService.Services
{
    public class DynamicUIBusiness: IDynamicUIBusiness
    {
        private IDynamicUIRepository _dynamicUIRepository;         
        public DynamicUIBusiness(IDynamicUIRepository dynamicUIRespository )
        {
            _dynamicUIRepository = dynamicUIRespository;
           
        }

        public List<Menu> GetAllMenues()
        {
            try
            {
                return _dynamicUIRepository.GetAllMenues();
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}