﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        //IEnumerable<Company>GetAllCompanies(bool trackChanges);

        Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);
        //Company GetCompany(Guid companyId, bool trackChanges);

        void CreateCompany(Company company);

        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        //IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        void DeleteCompany(Company company);
    }
}
