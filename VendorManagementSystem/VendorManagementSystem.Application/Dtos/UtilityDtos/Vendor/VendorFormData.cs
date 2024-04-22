using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor
{
    public class VendorFormData
    {
        public IEnumerable<VendorFormCategories> Categories { get; set; } = new List<VendorFormCategories>();
        public IEnumerable<VendorFormTypes> VednorTypes { get; set; } = new List<VendorFormTypes>();
    }
}
