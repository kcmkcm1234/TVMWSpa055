using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using UserInterface.Models;
using SAMTool.DataAccessObject.DTO;
using Microsoft.Practices.Unity;
using SAMTool.BusinessServices.Contracts;

namespace UserInterface.Controllers
{
    public class DynamicUIController : Controller
    {
        // GET: DynamicUI
        public string LoggedUserName { get; set; }
        private IDynamicUIBusiness _dynamicUIBusiness;
        [Dependency]
        public IUserBusiness _userBusiness { get; set; }
        public DynamicUIController(IDynamicUIBusiness dynamicUIBusiness )
        {
            _dynamicUIBusiness = dynamicUIBusiness;
           
        }

        public ActionResult _MenuNavBar()
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            LoggedUserName = _appUA.UserName;
            List<Menu> menulist = _dynamicUIBusiness.GetAllMenues();
            
            DynamicUIViewModel dUIObj = new DynamicUIViewModel();
            dUIObj.MenuViewModelList = Mapper.Map<List<Menu>, List<MenuViewModel>>(menulist);
            foreach (MenuViewModel item in dUIObj.MenuViewModelList)
            {
                if (item.SecurityObject != null)
                {
                    Permission _permission = _userBusiness.GetSecurityCode(LoggedUserName, item.SecurityObject);

                    if (_permission.AccessCode.Contains('R'))
                    {
                        item.HasAccess = true;
                    }
                }

            }
            foreach(MenuViewModel item in dUIObj.MenuViewModelList)
            {
                    if (item.SecurityObject == null)
                    {
                        foreach (var cp in dUIObj.MenuViewModelList.Where(p => p.ParentID == item.ID))
                        {
                            if (cp.HasAccess)
                            {
                                item.HasAccess = true;
                                break;
                            }
                        }
                    }
            }
            return View(dUIObj);
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UnderConstruction() {
            return View();
        }
    }
}