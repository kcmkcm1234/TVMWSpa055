using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Customer
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
        public bool IsInternalComp { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTitle { get; set; }
        public string Website { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string OtherPhoneNos { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentTermCode { get; set; }
        public string TaxRegNo { get; set; }
        public string PANNO { get; set; }
        public string GeneralNotes { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal OutStanding { get; set; }
        public Common commonObj { get; set; }
        public PaymentTerms PaymentTermsObj { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string PaymentTerm { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string PaymentDueDateFormatted { get; set; }
    }
}

    public class Titles
    {
        public string Title { get; set; }
    }
