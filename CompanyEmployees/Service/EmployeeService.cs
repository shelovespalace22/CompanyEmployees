using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;

        private readonly ILoggerManager _logger;

        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /* Obtener todos los empleados */
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges)
        {
            var employees = await _repository.Employee.GetAllEmployeesAsync(trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }

        /* Obtener un empleado por Id */
        public async Task<EmployeeDto> GetEmployeeAsync(Guid id, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(id, trackChanges);

            if (employee is null)
                throw new EmployeeNotFoundException(id);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        /* Obtener todos los empleados de una compañia */
        public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            if (!employeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, trackChanges);

            //var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

            //var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            //return employeesDto;

            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

            return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);

            
        }

        /* Obtener un empleado especifico por compañia */
        public async Task<EmployeeDto> GetEmployeeByCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, trackChanges);

            //var employeeDb = await _repository.Employee.GetEmployeeByCompanyAsync(companyId, id, trackChanges);

            //if (employeeDb is null)
            //    throw new EmployeeNotFoundException(id);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);

            return employee;
        }

        /* Crear un empleado */
        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);

            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        /* Eliminar un empleado */
        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, trackChanges);

            //var employeeForCompany = await _repository.Employee.GetEmployeeByCompanyAsync(companyId, id, trackChanges);

            //if (employeeForCompany is null)
            //    throw new EmployeeNotFoundException(id);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            _repository.Employee.DeleteEmployee(employeeDb);

            await _repository.SaveAsync();
        }

        /* Actualizar empleado con PUT */
        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, compTrackChanges);

            //var employeeEntity = await _repository.Employee.GetEmployeeByCompanyAsync(companyId, id, empTrackChanges);

            //if (employeeEntity is null)
            //    throw new EmployeeNotFoundException(id);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            _mapper.Map(employeeForUpdate, employeeDb);

            await _repository.SaveAsync();
        }

        /* Obtener empleado para PATCH */
        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);

            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);

            await CheckIfCompanyExists(companyId, compTrackChanges);

            //var employeeEntity = await _repository.Employee.GetEmployeeByCompanyAsync(companyId, id, empTrackChanges);

            //if (employeeEntity is null)
            //    throw new EmployeeNotFoundException(companyId);

            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);

            return (employeeToPatch: employeeToPatch, employeeEntity: employeeDb);
        }

        /* Actualizar empleado por PATCH */
        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);

            await _repository.SaveAsync();
        }



        /* ***** Métodos Privados ***** */


        /* Verificar si Existe la Compañia (registro) */

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }


        /* Obtener y Verificar existencia de Empleado (registro) */

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeByCompanyAsync(companyId, id, trackChanges);

            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);

            return employeeDb;
        }
    }
}
