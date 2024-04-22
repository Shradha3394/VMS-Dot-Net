using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class CategoryRespository : ICategoryRepository
    {
        private readonly DataContext _db;

        public CategoryRespository(DataContext db)
        {
           _db = db;
        }
        public Category? AddCategory(Category category)
        {
            _db.Catrogries.Add(category);
            int result = _db.SaveChanges();
            if(result>0)
            {
                return category;
            }
            return null;
        }

        public int DeleteCategory(int id)
        {
            int ans;
           var transaction = _db.Database.BeginTransaction();
            // removing foreign key dependency
            var mappings = _db.VendorCategoryMappings.Where(vcm => vcm.CategoryId == id).ToList();
            if(mappings != null)
            {
                foreach(var mapping in mappings)
                {
                    mapping.CategoryId = null;
                    mapping.Status = false;
                }
            }

            // now remove the category. 
            var category = _db.Catrogries.Where(c => c.Id == id).FirstOrDefault();
            if (category != null)
            {
                _db.Catrogries.Remove(category);
                ans = 1;
            }
            else
            {
                ans = 0;
            }
            _db.SaveChanges();
            transaction.Commit();
            return ans;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            IEnumerable<Category> catogiries = _db.Catrogries.ToList();
            return catogiries;
        }

        public Category? GetCategory(int id)
        {
            Category? category = _db.Catrogries.FirstOrDefault(c => c.Id == id);
            return category;
        }

        public IEnumerable<VendorFormCategories> GetCategoriesForForm()
        {
            IEnumerable<VendorFormCategories> categories =
                _db.Catrogries.Where(c => c.status == true)
                .Select(c => new VendorFormCategories
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
            return categories;
        }

        public bool hasCategoryByName(string name)
        {
            Category? category = _db.Catrogries.FirstOrDefault(c => c.Name == name);
            return category != null;
        }

        public Category? UpdateCategory(int id, CategoryUpdateDTO categoryDto)
        {
            Category? category = GetCategory(id);
            if (category == null) throw new Exception($"Can't find a category of id {id}");

            category.Name = categoryDto.Name;
            category.description = categoryDto.Description;
            category.UpdatedAt = categoryDto.UpdatedAt;
            category.UpdatedBy = categoryDto.UpdatedBy;

            int res = _db.SaveChanges();
            if(res>0)
            {
                return GetCategory(id);
            }
            return null;
        }
    }
}
