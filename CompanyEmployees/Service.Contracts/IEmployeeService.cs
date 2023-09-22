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
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges);

        /* Obtener un empleado por Id */
        Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, bool trackChanges);

        /* Obtener todos los empleados de una compañia */
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);

        /* Obtener un empleado especifico por compañia */
        Task<EmployeeDto> GetEmployeeByCompanyAsync(Guid companyId, Guid id, bool trackChanges);

        /* Crear un empleado */
        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);

        /* Eliminar un empleado */
        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);

        /* Actualizar empleado con PUT */
        Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);

        /* Obtener empleado para PATCH */
        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)>
            GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        /* Actualizar empleado por PATCH */
        Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
