using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace SPAccounts.DataAccessObject.DTO
{
    public class UploadedFiles
    {
        public string FileStatus { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int RecordCount { get; set; }
        public Guid ID { get; set; }
        public Common CommonObj { get; set; }
        public string CreatedDate { get; set; }

        public int RemovedDataCount { get; set; }
        public List<ImportOtherExpenses> ImportExpenseList { get; set; }
    }
}