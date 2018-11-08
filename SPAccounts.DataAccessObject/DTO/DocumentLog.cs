using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class DocumentLog
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string DocType { get; set; }
        public DateTime Date { get; set; }     
        public string Reason { get; set; }
        public Guid DocumentID { get; set; }
        public string DateFormatted { get; set; }
        public Common CommonObj { get; set; }
        public string OldValue { get; set; }
        public Guid ID { get; set; }
    }
}