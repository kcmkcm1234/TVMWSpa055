using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        IHomeBusiness _homeBusiness;
        public HomeController(IHomeBusiness home)
        {
            _homeBusiness = home;
        }
        // GET: Home
        public ActionResult Index()
        {
            List<HomeViewModel> SysLinks = Mapper.Map<List<Home>, List<HomeViewModel>>(_homeBusiness.GetAllSysLinks());
            return View(SysLinks);
        }
    }
}