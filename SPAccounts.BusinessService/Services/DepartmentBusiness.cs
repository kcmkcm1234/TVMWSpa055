using SPAccounts.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;

namespace SPAccounts.BusinessService.Services
{
    public class DepartmentBusiness : IDepartmentBusiness
    {
        IDepartmentRepository _departmentRepository;
        public DepartmentBusiness(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public object DeleteDepartment(string Code)
        {
           return  _departmentRepository.DeleteDepartment(Code);
        }

        public List<Department> GetAllDetpartments()
        {
            List<Department> DepartmentList = null;
            try
            {
                DepartmentList = _departmentRepository.GetAllDetpartments();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return DepartmentList;
        }

        public Department GetDepartmentDetails(string Code)
        {
            List<Department> DepartmentList = null;
            Department department = null;
            try
            {
                DepartmentList = GetAllDetpartments();
                department = DepartmentList != null ? DepartmentList.Where(D => D.Code == Code).SingleOrDefault() : null;

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return department;
        }

        public object InsertDepartment(Department department)
        {
           return _departmentRepository.InsertDepartment(department);
        }

        public object UpdateDepartment(Department department)
        {
            return _departmentRepository.UpdateDepartment(department);
        }
    }
}