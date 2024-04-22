
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Models.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public int VendorTypeId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public string ContactPersonNumber { get; set; } = string.Empty;
        public string ContactPersonEmail { get; set; } = string.Empty;
        public string RelationshipDuration {  get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; } = String.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public VendorType VendorType { get; set; }

        public ICollection<VendorCategoryMapping> VendorCategoryMapping { get; set; }
    }
}
