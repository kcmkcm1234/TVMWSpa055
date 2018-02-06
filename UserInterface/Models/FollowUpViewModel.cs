using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class FollowUpViewModel
    {

        public Guid ID { get; set; }
        public Guid CustomerID { get; set; }
        [Required(ErrorMessage = "FollowUp Date is missing")]
        [Display(Name = "FollowUp Date")]
        public string FollowUpDate { get; set; }
        [Required(ErrorMessage = "FollowUp Time is missing")]
        [Display(Name = "FollowUp Time")]
        public string FollowUpTime { get; set; }
        public string HdnFollowUpTime { get; set; }
        [Required(ErrorMessage = "Remarks is missing")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Display(Name = "Reminder Type")]
        public string ReminderType { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Contact No")]
        public string ContactNO { get; set; }
        public CommonViewModel commonObj { get; set; }
        [Display(Name = "Min")]
        public string Minutes { get; set; }
    }

        public class FollowUpListViewModel
        {
            public Guid FlwID { get; set; }
            public List<FollowUpViewModel> FollowUpList { get; set; }
        }
    
}