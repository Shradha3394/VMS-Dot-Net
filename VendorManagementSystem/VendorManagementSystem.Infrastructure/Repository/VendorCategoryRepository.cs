using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Models.Models;
using VendorManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace VendorManagementSystem.Infrastructure.Repository
{
    public class VendorCategoryRepository : IVendorCategoryRepository
    {
        private readonly DataContext _db;

        public VendorCategoryRepository(DataContext db)
        {
            _db = db;
        }

        public bool AddVendorCategoryEntry(List<VendorCategoryMapping> entries)
        {
            _db.VendorCategoryMappings.AddRange(entries);
            int changes = _db.SaveChanges(); //happens in a transaction default
            return changes > 0;
        }
        public bool RemoveAllMappingsByVendorId(int vendorId)
        {
            /* List<VendorCategoryMapping> extrEntries = _db.VendorCategoryMappings.Where(v => v.VendorId == vendorId).ToList();
            _db.RemoveRange(extrEntries);
            int change = _db.SaveChanges(); */
            _db.VendorCategoryMappings.Where(v => v.VendorId == vendorId).ExecuteDelete();
            int change = _db.SaveChanges();
            return change > 0;  

        }
    }
}
