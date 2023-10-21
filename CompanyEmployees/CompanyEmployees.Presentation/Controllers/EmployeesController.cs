using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers
{
    //[Route("api/employees")]
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeesController(IServiceManager service) =>
            _service = service;

        /* Obtener todos los empleados */
        [HttpGet("/api/employees")]
        public async Task<IActionResult> GetEmployees()
        {

            var allEmployees = await _service.EmployeeService.GetAllEmployeesAsync(trackChanges: false);

            return Ok(allEmployees);
        }

        /* Obtener un empleado por Id */
        [HttpGet("/api/employees/{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _service.EmployeeService.GetEmployeeAsync(id, trackChanges: false);

            return Ok(employee);
        }

        /* Obtener todos los empleados de una compañia */
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.employees);

            //var employees = await _service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            //return Ok(employees);
        }

        /* Obtener un empleado especifico por compañia */
        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = await _service.EmployeeService.GetEmployeeByCompanyAsync(companyId, id, trackChanges: false);

            return Ok(employee);
        }

        /* Crear un empleado */
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            //if (employee is null)
            //    return BadRequest("EmployeeForCreationDto object is null");

            //if (!ModelState.IsValid) 
            //    return UnprocessableEntity(ModelState);

            //ModelState.ClearValidationState(nameof(EmployeeForCreationDto));

            //if (!TryValidateModel(employee, nameof(EmployeeForCreationDto)))
            //    return UnprocessableEntity(ModelState);

            var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        /* Eliminar un empleado */
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);

            return NoContent();
        }

        /* Actualizar empleado con PUT */
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            //if (employee is null)
            //    return BadRequest("EmployeeForUpdateDto object is null");

            //if (!ModelState.IsValid) 
            //    return UnprocessableEntity(ModelState);

            await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }

        /* Actualizar empleado por PATCH */
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);

            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid) 
                return UnprocessableEntity(ModelState);

            await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);

            return NoContent();
        }
    }
}
