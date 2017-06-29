using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class UA
    {
        public string UserName { get; set; }
        public string SCCode { get; set; }
        public DateTime DateTime { get; set; }
        public Guid? UserID { get; set; }


        public UA()
        {
            try
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["TvmValid"] != null))
                {
                    UA uaObj = (UA)HttpContext.Current.Session["TvmValid"];
                    UserName = uaObj.UserName;
                    SCCode = uaObj.SCCode;
                    UserID = uaObj.UserID;
                    uaObj.DateTime = GetCurrentDateTime();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DateTime CurrentDatetime()
        {
            return DateTime.Now;
        }
        public DateTime GetCurrentDateTime()
        {
            string tz = System.Web.Configuration.WebConfigurationManager.AppSettings["TimeZone"];
            DateTime DateNow = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            return (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateNow, tz));
        }
    }
}