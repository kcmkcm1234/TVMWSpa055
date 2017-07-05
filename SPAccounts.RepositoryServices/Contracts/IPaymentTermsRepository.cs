using SPAccounts.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Contracts
{
    public interface IPaymentTermsRepository
    {
        List<PaymentTerms> GetAllPayTerms();
    }
}