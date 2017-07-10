using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class SAMPanelController : Controller
    {
      //  public string ReadAccess;
        IHomeBusiness _homeBusiness;
        public SAMPanelController(IHomeBusiness home)
        {
            _homeBusiness = home;
        }
    
       // [AuthSecurityFilter(ProjectObject = "SAMPanel", Mode = "R")]
        public ActionResult Index()
        {
           // AppUA _appUA= Session["AppUA"] as AppUA;
          //  Permission _permission = Session["UserRights"] as Permission;
           // ReadAccess = _permission.SubPermissionList.First(s => s.Name == "LHS").AccessCode;
           // string R = _permission.SubPermissionList.First(s => s.Name == "RHS").AccessCode;

            List<HomeViewModel> SysLinks = Mapper.Map<List<Home>, List<HomeViewModel>>(_homeBusiness.GetAllSysLinks());
           // Session.Remove("UserRights");
            return View(SysLinks);
        }
    }
}