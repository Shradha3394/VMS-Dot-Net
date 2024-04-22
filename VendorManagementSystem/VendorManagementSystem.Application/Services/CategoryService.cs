using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IErrorLoggingService _errorLog;
        private readonly ITokenService _tokenService;

        public CategoryService(ICategoryRepository categoryRepository, IErrorLoggingService errorLog, ITokenService tokenService)
        {
            _categoryRepository = categoryRepository;
            _errorLog = errorLog;
            _tokenService = tokenService;
        }

        public ApplicationResponseDTO<Category> CreateCategory(CategoryDTO categoryDto, string? token)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }
                if (_categoryRepository.hasCategoryByName(categoryDto.Name))
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.DuplicateEntryError,
                            Message = new List<string> { $"Category Name: {categoryDto.Name} already present" },
                        }
                    };
                }
                var currentUser = _tokenService.ExtractUserDetials(token!, "username");
                Category category = new Category
                {
                    Name = categoryDto.Name,
                    description = categoryDto.Description,
                    status = true,
                    CreatedBy = currentUser,
                    UpdatedBy = currentUser,

                };
                var result = _categoryRepository.AddCategory(category);
                if (result != null)
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Data = result,
                    };
                }
                else
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.DatabaseError,
                            Message = new List<string> { "Error While adding entry to Database" },
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);

                return new ApplicationResponseDTO<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDTO<object> DeleteCategory(int id)
        {
            try
            {
                var result = _categoryRepository.DeleteCategory(id);
                if(result==1)
                {
                    return new ApplicationResponseDTO<object>
                    {
                        Data = null,
                        Message = "Deletion is successful",
                    };
                }
                else
                {
                    return new ApplicationResponseDTO<object>
                    {
                        Data = null,
                        Message = "No category found to delete",
                    };
                }
            } catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<object>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDTO<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var result = _categoryRepository.GetAllCategories();
                return new ApplicationResponseDTO<IEnumerable<Category>>()
                {
                    Data = result,
                };

            }catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<IEnumerable<Category>>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }

        public ApplicationResponseDTO<Category> GetCategoryById(int id)
        {
            try
            {
                Category? category = _categoryRepository.GetCategory(id);
                if(category == null)
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Error = new Error
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { $"Category with id {id} not found" },
                        }
                    };
                }
                else
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Data = category,
                    };
                }
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }

        public ApplicationResponseDTO<Category> UpdateCategoryById(int id, CategoryDTO categoryDto, string? token)
        {
            try
            {
                if (token.IsNullOrEmpty())
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InvalidInputFields,
                            Message = new List<string> { $"The jwt token is null" },
                        }
                    };
                }
                var currentUser = _tokenService.ExtractUserDetials(token!, "username");
                CategoryUpdateDTO categoryUpdate = new()
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    UpdatedBy = currentUser,
                };
                var res = _categoryRepository.UpdateCategory(id, categoryUpdate);
                if(res != null)
                {
                    return new ApplicationResponseDTO<Category>
                    {
                        Data = res,
                    };
                }
                return new ApplicationResponseDTO<Category>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { "Error While adding entry to Database" },
                    }
                };
            }
            catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<Category>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message },
                    }
                };
            }
        }
    }
}
