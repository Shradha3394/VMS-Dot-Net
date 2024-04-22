using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface ICategoryRepository
    {
        public Category? AddCategory (Category category);
        public Category? GetCategory (int id);
        public IEnumerable<Category> GetAllCategories ();
        public int DeleteCategory (int id);
        public bool hasCategoryByName (string name);
        public Category? UpdateCategory(int id, CategoryUpdateDTO categoryDto);
        public IEnumerable<VendorFormCategories> GetCategoriesForForm();
    }
}
