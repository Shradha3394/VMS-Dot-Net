using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos
{
    public class ValidityCheckDTO
    {
        public bool status;
        public readonly Dictionary<string, string> errors;

        public ValidityCheckDTO()
        {
            status = true;
            errors = new Dictionary<string, string>();
        }

        public void AddError(string type, string error)
        {
            errors[type] = error;
        }
    }
}
