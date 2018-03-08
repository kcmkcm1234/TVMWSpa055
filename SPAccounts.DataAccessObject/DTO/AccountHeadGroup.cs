using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class AccountHeadGroup
    {
        public Guid ID { get; set; }
        public string GroupName { get; set; }
        public string AccountHead { get; set; }
        public string AccountHeads { get; set; }
        public Common commonObj { get; set; }
        public bool IsGrouped { get; set; }
         }

    public class AccountHeadGroupDetail
    {
        public Guid ID { get; set; }
        public Guid GroupID { get; set; }
        public string AccountHeadCode { get; set; }
    }
}