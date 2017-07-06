using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
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
        IHomeBusiness _homeBusiness;
        public SAMPanelController(IHomeBusiness home)
        {
            _homeBusiness = home;
        }
    
        //[AuthSecurityFilter(ProjectObject = "SAMPanel", Mode = "R")]
        public ActionResult Index()
        {
            List<HomeViewModel> SysLinks = Mapper.Map<List<Home>, List<HomeViewModel>>(_homeBusiness.GetAllSysLinks());
            return View(SysLinks);
        }
    }
}