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
    public class SupplierController : ApiController
    {
        #region Constructor_Injection

        ISupplierBusiness _supplierBusiness;


        public SupplierController(ISupplierBusiness supplierBusiness)
        {
            _supplierBusiness = supplierBusiness;

        }
        #endregion Constructor_Injection

        Const messages = new Const();

        #region GetAllSuppliersForMobile
        [HttpPost]
        public string GetAllSuppliersDetailForMobile()
        {
            try
            {
                List<SuppliersViewModel> suppliersList = Mapper.Map<List<Supplier>, List<SuppliersViewModel>>(_supplierBusiness.GetAllSuppliersForMobile());
                return JsonConvert.SerializeObject(new { Result = true, Records = suppliersList });
            }
            catch (Exception ex)
            {
               
                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion  GetAllSuppliersForMobile


        #region GetSupplierDetailsByID
        [HttpPost]
        public string GetSupplierDetailsByIDForMobile(Supplier sup)
        {
            try
            {

                SuppliersViewModel supplierObj = Mapper.Map<Supplier, SuppliersViewModel>(_supplierBusiness.GetSupplierDetailsForMobile(sup.ID != null && sup.ID.ToString() != "" ? Guid.Parse(sup.ID.ToString()) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = true, Records = supplierObj });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = false, Message = ex.Message });
            }
        }
        #endregion  GetSupplierDetailsByIDForMobile
    }
}
