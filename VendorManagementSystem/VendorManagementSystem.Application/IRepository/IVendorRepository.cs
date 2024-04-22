using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IVendorRepository
    {
        IDbContextTransaction BeginTransaction();
        int AddVendor(Vendor vendor);
        Vendor? GetVendorById(int vendorId);
        IEnumerable<Vendor> GetVendors(int pageNumber, int pageSize);
        bool UpdateVendor(Vendor vendor, CreateVendorDTO vendorDTO, string currentUser);
        bool ToggleStatus(Vendor vendor);
        IEnumerable<VendorFormTypes> GetTypesForForm();
    }
}
