using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

        //[HttpGet]
        //public IActionResult GetEmployees()
        //{

        //    var allEmployees = _service.EmployeeService.GetAllEmployees(trackChanges: false);

        //    return Ok(allEmployees);
        //}

        //[HttpGet("{id:guid}")]
        //public IActionResult GetEmployee(Guid id)
        //{
        //    var employee = _service.EmployeeService.GetEmployee(id, trackChanges: false);

        //    return Ok(employee);
        //}

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);

            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

            return Ok(employee);
        }
    }
}
