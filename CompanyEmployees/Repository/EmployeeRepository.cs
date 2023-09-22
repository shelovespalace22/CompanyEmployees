using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {
            
        }

        /* Operaciones Asincronas */

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();

        public async Task<Employee> GetEmployeeAsync(Guid employeeId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(employeeId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) =>
            await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();

        public async Task<Employee> GetEmployeeByCompanyAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        ///* Obtener todos los empleados */
        //public IEnumerable<Employee> GetAllEmployees(bool trackChanges) =>
        //    FindAll(trackChanges)
        //    .OrderBy(e => e.Name)
        //    .ToList();

        ///* Obtener empleado por Id */
        //public Employee GetEmployee(Guid employeeId, bool trackChanges) =>
        //    FindByCondition(e => e.Id.Equals(employeeId), trackChanges)
        //    .SingleOrDefault();

        ///* Obtener todos los empleados de una compañia */
        //public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
        //    FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
        //    .OrderBy(e => e.Name)
        //    .ToList();

        ///* Obtener un empleado especifico por compañia */
        //public Employee GetEmployeeByCompany(Guid companyId, Guid id, bool trackChanges) =>
        //    FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
        //    .SingleOrDefault();

        /* Crear empleado */
        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId; Create(employee);
        }

        /* Eliminar empleado */
        public void DeleteEmployee(Employee employee) => Delete(employee);
    }
}
