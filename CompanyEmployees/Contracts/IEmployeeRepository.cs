using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        /* Obtener todos los empleados */
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);

        /* Obtener empleado por Id */
        Employee GetEmployee(Guid employeeId, bool trackChanges);

        /* Obtener todos los empleados de una compañia */
        IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);

        /* Obtener un empleado especifico por compañia */
        Employee GetEmployeeByCompany(Guid companyId, Guid id, bool trackChanges);

        /* Crear empleado */
        void CreateEmployeeForCompany(Guid companyId, Employee employee);

        /* Eliminar empleado */
        void DeleteEmployee(Employee employee);
    }
}
