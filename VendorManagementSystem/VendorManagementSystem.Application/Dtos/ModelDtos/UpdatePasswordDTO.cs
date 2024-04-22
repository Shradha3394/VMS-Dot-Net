using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos
{
    public class UpdatePasswordDTO
    {
        [Required]
        [EmailAddress]
        public string  Email{ get; set; }
        [Required]
        public string Password { get; set; }
    }
}
