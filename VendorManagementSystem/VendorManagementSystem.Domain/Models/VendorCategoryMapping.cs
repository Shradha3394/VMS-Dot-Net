using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Models.Models
{
    public class VendorCategoryMapping
    {
        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }
        [Required]
        public Vendor Vendor { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        [Required]
        public Category Category { get; set; }

        public bool Status { get; set; } = true;
    }

}
