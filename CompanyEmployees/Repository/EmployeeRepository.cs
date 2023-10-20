using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {
            
        }

        /* Operaciones Asincronas */

        /* Obtener todos los Empleados */
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();

        /* Obteenr Empleado por Id */
        public async Task<Employee> GetEmployeeAsync(Guid employeeId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(employeeId), trackChanges)
            .SingleOrDefaultAsync();

        /* Obtener todos los Empleados de una Compañia */
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,  EmployeeParameters employeeParameters, bool trackChanges)
        {

            /* CÓDIGO OBSOLETO */

            //var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            //    .OrderBy(e => e.Name)
            //    .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            //    .Take(employeeParameters.PageSize)
            //    .ToListAsync();

            //var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();

            //return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);

            //return PagedList<Employee>
            //    .ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);

            //var employees = await FindByCondition(e => e.CompanyId.Equals(companyId) && (e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge), trackChanges)
            //    .OrderBy(e => e.Name)
            //    .ToListAsync();

            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .OrderBy(e => e.Name)
                .ToListAsync();

            return PagedList<Employee>
                .ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);

        }

        /* CÓDIGO OBSOLETO */

            //await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            //.OrderBy(e => e.Name)
            //.Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
            //.Take(employeeParameters.PageSize)
            //.ToListAsync();

        /* Obtener un Empleado especifico por Compañia */
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
