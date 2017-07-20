using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class SupplierCreditNote
    {
        public Guid ID { get; set; }
        public Supplier supplier { get; set; }
        public Companies Company { get; set; }

        public string CRNRefNo { get; set; }
        public string CRNDate { get; set; }

        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string GeneralNotes { get; set; }
        
    }
}