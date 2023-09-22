﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> GetCompanies() 
        {
            //throw new Exception("Exception");

            var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id) 
        {
            var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            
            return Ok(company); 
        }

        [HttpPost] 
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company) 
        {
            if (company is null) 
                return BadRequest("CompanyForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            ModelState.ClearValidationState(nameof(CompanyForCreationDto));

            if (!TryValidateModel(company, nameof(CompanyForCreationDto)))
                return UnprocessableEntity(ModelState);

            var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids) 
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            
            return Ok(companies); 
        }

        [HttpPost("collection")] 
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection) 
        {
            var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id) 
        {
            await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            
            return NoContent(); 
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company) 
        {
            if (company is null) 
                return BadRequest("CompanyForUpdateDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            
            return NoContent(); 
        }

        /* Actualizar compañia por PATCH */

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _service.CompanyService.GetCompanyForPatchAsync(id, trackChanges: true);

            patchDoc.ApplyTo(result.companyToPatch, ModelState);

            TryValidateModel(result.companyToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.CompanyService.SaveChangesForPatchAsync(result.companyToPatch, result.companyEntity);

            return NoContent();
        }
    }
}
