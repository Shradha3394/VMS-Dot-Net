using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos
{
    public class CreateVendorDTO
    {
        public string OrganizationName { get; set; }
        public int VendorTypeId { get; set; }
        public string Address { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmail { get; set; }
        public string RelationshipDuration { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
