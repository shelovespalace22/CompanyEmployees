using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId)
            :base($"The Employee with ID: {employeeId} doesn´t exist in the database.")
        {
            
        }
    }
}
