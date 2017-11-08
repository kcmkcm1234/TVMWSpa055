using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccounts.RepositoryServices.Contracts
{
   public interface ISupplierRepository
    {
        List<Supplier> GetAllSuppliers();
        List<Supplier> GetAllSuppliersForMobile(Supplier supObj);
        Supplier GetSupplierDetailsForMobile(Guid ID);
        Supplier GetSupplierDetails(Guid ID);
        Supplier InsertSupplier(Supplier _supplierObj);
        object UpdateSupplier(Supplier _supplierObj);
        object DeleteSupplier(Guid ID);
       

    }
}
