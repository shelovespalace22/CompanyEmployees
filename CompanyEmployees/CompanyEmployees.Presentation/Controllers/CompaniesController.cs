﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) =>
            _service = service;

        [HttpGet] 
        public IActionResult GetCompanies() 
        {
            //throw new Exception("Exception");

            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
            
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id) 
        {
            var company = _service.CompanyService.GetCompany(id, trackChanges: false);
            
            return Ok(company); 
        }

        [HttpPost] 
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company) 
        {
            if (company is null) 
                return BadRequest("CompanyForCreationDto object is null");
            
            var createdCompany = _service.CompanyService.CreateCompany(company);
            
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids) 
        {
            var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
            
            return Ok(companies); 
        }

        [HttpPost("collection")] 
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection) 
        {
            var result = _service.CompanyService.CreateCompanyCollection(companyCollection);
            
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id) 
        {
            _service.CompanyService.DeleteCompany(id, trackChanges: false);
            
            return NoContent(); 
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company) 
        {
            if (company is null) return BadRequest("CompanyForUpdateDto object is null");
            
            _service.CompanyService.UpdateCompany(id, company, trackChanges: true);
            
            return NoContent(); 
        }

        /* Actualizar compañia por PATCH */

        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = _service.CompanyService.GetCompanyForPatch(id, trackChanges: true);

            patchDoc.ApplyTo(result.companyToPatch);

            _service.CompanyService.SaveChangesForPatch(result.companyToPatch, result.companyEntity);

            return NoContent();
        }
    }
}
