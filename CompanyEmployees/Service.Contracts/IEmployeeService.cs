using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        /* Obtener todos los empleados */
        IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges);

        /* Obtener un empleado por Id */
        EmployeeDto GetEmployee(Guid employeeId, bool trackChanges);

        /* Obtener todos los empleados de una compañia */
        IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);

        /* Obtener un empleado especifico por compañia */
        EmployeeDto GetEmployeeByCompany(Guid companyId, Guid id, bool trackChanges);

        /* Crear un empleado */
        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);

        /* Eliminar un empleado */
        void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);

        /* Actualizar empleado con PUT */
        void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

        /* Obtener empleado para PATCH */
        (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        /* Actualizar empleado por PATCH */
        void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
