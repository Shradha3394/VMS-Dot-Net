using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface ICategoryService
    {
        public ApplicationResponseDTO<Category> CreateCategory(CategoryDTO categoryDto, string? token);
        public ApplicationResponseDTO<IEnumerable<Category>> GetAllCategories();
        public ApplicationResponseDTO<Category> GetCategoryById(int id);
        public ApplicationResponseDTO<Category> UpdateCategoryById(int id, CategoryDTO categoryDto, string? token);

        public ApplicationResponseDTO<object> DeleteCategory(int id);
    }
}
