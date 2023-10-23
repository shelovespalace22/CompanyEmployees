using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
}
