using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class DocumentLogViewModel
    {
        public int Code { get; set; }
        public string DocumentNo { get; set; }
        public string DocType { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Reason is missing")]
        public string Reason { get; set; }
        public Guid DocumentID { get; set; }
        public string DateFormatted { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public string OldValue { get; set; }
        public Guid ID { get; set; }
    }
}