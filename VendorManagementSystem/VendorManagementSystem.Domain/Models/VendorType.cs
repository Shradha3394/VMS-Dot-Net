using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Models.Models
{
    public class VendorType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Vendor> Vendors { get; } = new List<Vendor>();
    }
}
