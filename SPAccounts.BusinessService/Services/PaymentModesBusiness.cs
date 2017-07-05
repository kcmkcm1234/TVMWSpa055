using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class PaymentModesBusiness : IPaymentModesBusiness
    {
        private IPaymentModesRepository _pmRepository;

        public PaymentModesBusiness(IPaymentModesRepository pmRepository)
        {
            _pmRepository = pmRepository;
        }

        public List<PaymentModes> GetAllPaymentModes()
        {
            return _pmRepository.GetAllPaymentModes();
        }
    }
}