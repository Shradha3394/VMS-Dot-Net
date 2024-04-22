using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public  interface IVendorService
    {

        ApplicationResponseDTO<int> CreateVendor(CreateVendorDTO userDto, string currentUser);
        ApplicationResponseDTO<IEnumerable<Vendor>> GetAllVendors(int pageNumber, int pageSize);
        ApplicationResponseDTO<bool> UpdateVendor(int vendorId,CreateVendorDTO userDto, string currentUser);
        ApplicationResponseDTO<Vendor> GetVendorById(int vendorId);

        ApplicationResponseDTO<bool> ToogleVendorStatus(int vendorId);

        ApplicationResponseDTO<VendorFormData> GetFormData();
    }
}
