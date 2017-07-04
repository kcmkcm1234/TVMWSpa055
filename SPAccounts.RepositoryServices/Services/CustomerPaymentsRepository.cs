﻿using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class CustomerPaymentsRepository:ICustomerPaymentsRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CustomerPaymentsRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

    }
}