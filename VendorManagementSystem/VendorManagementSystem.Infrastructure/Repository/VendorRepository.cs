using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Models.Models;
using VendorManagementSystem.Infrastructure.Data;
using MimeKit.Tnef;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly DataContext _db;

        public VendorRepository(DataContext db)
        {
            _db = db;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public int AddVendor(Vendor vendor)
        {
            _db.Vendors.Add(vendor);
            _db.SaveChanges();
            return vendor.Id;
        }

        public Vendor? GetVendorById(int vendorId)
        {
            Vendor? vendor = _db.Vendors.FirstOrDefault(v => v.Id == vendorId);
            return vendor;
        }



        public IEnumerable<Vendor> GetVendors(int pageNumber, int pageSize)
        {
            var skipCount = pageSize * (pageNumber - 1);
            var vendors = _db.Vendors.OrderByDescending(v => v.Id).Skip(skipCount).Take(pageSize).ToList();
            return vendors;
        }

        public bool UpdateVendor(Vendor vendor,CreateVendorDTO vendorDTO, string currentUser)
        {
            vendor.OrganizationName = vendorDTO.OrganizationName;
            vendor.VendorTypeId = vendorDTO.VendorTypeId;
            vendor.Address = vendorDTO.Address;
            vendor.ContactPersonName = vendorDTO.ContactPersonName;
            vendor.ContactPersonNumber = vendorDTO.ContactPersonNumber;
            vendor.ContactPersonEmail = vendorDTO.ContactPersonEmail;
            vendor.RelationshipDuration = vendorDTO.RelationshipDuration;
            vendor.UpdatedAt = DateTime.UtcNow;
            vendor.UpdatedBy = currentUser;
            int change =  _db.SaveChanges();
            return change > 0;
        }
        public  bool ToggleStatus(Vendor vendor)
        {
            vendor.Status = !vendor.Status;
            int change = _db.SaveChanges();
            return change > 0;
        }

        public IEnumerable<VendorFormTypes> GetTypesForForm()
        {
            IEnumerable<VendorFormTypes> types =
                    _db.VendorTypes.Select(t => new VendorFormTypes { Id = t.Id, Name = t.Name, }).ToList();
            return types;
        }
    }
}
