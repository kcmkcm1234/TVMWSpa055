
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserInterface.Models
{
    public class UploadedFilesViewModel
    {
        public string FileStatus { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int RecordCount { get; set; }
        public Guid ID { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public string CreatedDate { get; set; }

        public int RemovedDataCount { get; set; }
        public List<ImportOtherExpensesViewModel> ImportExpenseList { get; set; }
    }
}