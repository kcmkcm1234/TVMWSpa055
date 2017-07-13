using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class OtherExpenseViewModel
    {
        [Display(Name = "Date")]
        public string CRNDateFormatted { get; set; }
    }
}