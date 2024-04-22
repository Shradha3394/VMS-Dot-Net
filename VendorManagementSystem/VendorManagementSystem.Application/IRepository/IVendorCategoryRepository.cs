using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public  interface IVendorCategoryRepository
    {
        bool AddVendorCategoryEntry(List<VendorCategoryMapping> entries);
        public bool RemoveAllMappingsByVendorId(int vendorId);


    }
}
