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

        public List<Supplier> GetAllSuppliersForMobile()
        {
            try
            {
                return _supplierRepository.GetAllSuppliersForMobile( );
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Supplier GetSupplierDetailsForMobile(Guid ID)
        {
            try
            {
                return _supplierRepository.GetSupplierDetailsForMobile(ID);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Supplier GetSupplierDetails(Guid ID)
        {
            return _supplierRepository.GetSupplierDetails(ID);
        }
        public object InsertUpdateSupplier(Supplier _supplierObj)
        {
            object result = null;
            try
            {
                if (_supplierObj.ID == Guid.Empty)
                {
                    result = _supplierRepository.InsertSupplier(_supplierObj);
                }
                else
                {
                    result = _supplierRepository.UpdateSupplier(_supplierObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteSupplier(Guid ID)
        {
            object result = null;
            try
            {
                result = _supplierRepository.DeleteSupplier(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}