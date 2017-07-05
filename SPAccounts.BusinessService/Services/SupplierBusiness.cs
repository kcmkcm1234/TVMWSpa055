using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class SupplierBusiness : ISupplierBusiness
    {
        private ISupplierRepository _supplierRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierBusiness(ISupplierRepository supplierRepository, ICommonBusiness commonBusiness)
        {
            _supplierRepository = supplierRepository;
            _commonBusiness = commonBusiness;

        }

        public List<Supplier> GetAllSuppliers()
        {
            try
            {
                return _supplierRepository.GetAllSuppliers();
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}