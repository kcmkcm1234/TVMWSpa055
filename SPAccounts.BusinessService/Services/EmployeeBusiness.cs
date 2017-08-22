using SPAccounts.BusinessService.Contracts;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.BusinessService.Services
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeBusiness(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public List<Employee> GetAllEmployees()
        {
            List<Employee> empList = new List<Employee>();
            empList = _employeeRepository.GetAllEmployees();
            empList = empList != null ? empList.Where(e => e.employeeTypeObj.Code =="EMP").ToList() : null;
            return empList;
        }

        public List<Employee> GetAllOtherEmployees()
        {
            List<Employee> empList = new List<Employee>();
            empList = _employeeRepository.GetAllEmployees();
            empList = empList != null ? empList.Where(e => e.employeeTypeObj.Code != "EMP").ToList() : null;
            return empList;
        }

        public List<EmployeeType> GetAllEmployeeTypes()
        {
            return _employeeRepository.GetAllEmployeeTypes();
        }
        public List<EmployeeType> GetAllDepartment()
        {
            return _employeeRepository.GetAllDepartment();
        }

        public List<EmployeeType> GetAllCategory()
        {
            return _employeeRepository.GetAllCategory();
        }

        public Employee GetEmployeeDetails(Guid ID)
        {
            return _employeeRepository.GetEmployeeDetails(ID);
        }

        public object InsertUpdateEmployee(Employee _employeeObj)
        {
            object result = null;
            try
            {
                if (_employeeObj.ID == Guid.Empty)
                {
                    result = _employeeRepository.InsertEmployee(_employeeObj);
                }
                else
                {
                    result = _employeeRepository.UpdateEmployee(_employeeObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteEmployee(Guid ID)
        {
            object result = null;
            try
            {
                result = _employeeRepository.DeleteEmployee(ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

       
    }
}