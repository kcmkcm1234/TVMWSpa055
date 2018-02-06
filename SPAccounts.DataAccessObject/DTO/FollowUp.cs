using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class FollowUp
    {
        public Guid ID { get; set; }
        public Guid CustomerID { get; set; }
        public string FollowUpDate { get; set; }
        public string FollowUpTime { get; set; }
        public string HdnFollowUpTime { get; set; }
        public string Remarks { get; set; }
        public string ContactName { get; set; }
        public string ContactNO { get; set; }
        public string Status { get; set; }
        public string Company { get; set; }
        public Common CommonObj { get; set; }
        public string Minutes { get; set; }

    }
}