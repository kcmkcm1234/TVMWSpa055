using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Configuration;

using System.Web.Mvc;
using UserInterface.Models;

namespace UserInterface.API
{
    public class LoginController : System.Web.Http.ApiController
    {
        Const _const = new Const();
        IUserBusiness _userBusiness;
        Guid AppID = Guid.Parse(ConfigurationManager.AppSettings["ApplicationID"]);
        public LoginController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        #region Login
        [HttpPost]
        public string HomeLogin(LoginViewModel loginvm)
        {
            UserViewModel uservm = null;
            if (!string.IsNullOrEmpty(loginvm.LoginName) && !string.IsNullOrEmpty(loginvm.Password))
            {

                uservm = Mapper.Map<User, UserViewModel>(_userBusiness.CheckUserCredentials(Mapper.Map<LoginViewModel, User>(loginvm)));
                if (uservm != null)
                {
                    return JsonConvert.SerializeObject(new { Result = true, Records = new { ID = uservm.ID, LoginName = uservm.LoginName, RoleCSV = uservm.RoleCSV, RoleDCSV = uservm.RoleIDCSV, UserName = uservm.UserName } });
                }

                else 
                {
                    return JsonConvert.SerializeObject(new { Result = false, Message = "Invalid Credentials " });
                }
            }


            else
            {
                return JsonConvert.SerializeObject(new { Result = false, Message = "Login Credentials Required" });
            }
            }
        }
    }



#endregion Login          
