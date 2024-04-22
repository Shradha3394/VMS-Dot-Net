using System.Numerics;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly ITokenService _tokenService;
        private readonly IErrorLoggingService _errorLog;
        private readonly IVendorCategoryRepository _vendorCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public VendorService(IVendorRepository vendorRepository, ITokenService tokenService, IErrorLoggingService errorLog, IVendorCategoryRepository vendorCategoryRepository, ICategoryRepository categoryRepository)
        {
            _vendorRepository = vendorRepository;
            _tokenService = tokenService;
            _errorLog = errorLog;
            _vendorCategoryRepository = vendorCategoryRepository;
            _categoryRepository = categoryRepository;
        }


        public ApplicationResponseDTO<int> CreateVendor(CreateVendorDTO vendorDTO, string jwtToken)
        {
            string currentUser = _tokenService.ExtractUserDetials(jwtToken, "email");
            Vendor vendor = new()
            {
                OrganizationName = vendorDTO.OrganizationName,
                VendorTypeId = vendorDTO.VendorTypeId,
                Address = vendorDTO.Address,
                ContactPersonName = vendorDTO.ContactPersonName,
                ContactPersonNumber = vendorDTO.ContactPersonNumber,
                ContactPersonEmail = vendorDTO.ContactPersonEmail,
                RelationshipDuration = vendorDTO.RelationshipDuration,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = currentUser,
            };

            using (var transaction = _vendorRepository.BeginTransaction())
            {
                try
                {
                    int vendorId = _vendorRepository.AddVendor(vendor);
                    var entries = vendorDTO.CategoryIds.Select(categoryId => new VendorCategoryMapping
                    {
                        VendorId = vendorId,
                        CategoryId = categoryId,
                        Status = true
                    }).ToList();
                    _vendorCategoryRepository.AddVendorCategoryEntry(entries);
                    transaction.Commit();
                    return new ApplicationResponseDTO<int>
                    {
                        Data = vendorId,
                    };


                }
                catch (Exception ex)
                {
                    _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                    transaction.Rollback();
                    return new ApplicationResponseDTO<int>
                    {
                        Data = -1,
                        Error = new()
                        {
                            Code = (int)ErrorCodes.InternalError,
                            Message = new List<string> { ex.Message },
                        },
                    };
                }
            }

        }



        public ApplicationResponseDTO<IEnumerable<Vendor>> GetAllVendors(int pageNumber, int pageSize)
        {
            try
            {
                var vendor = _vendorRepository.GetVendors(pageNumber,pageSize);
                if (vendor == null || !vendor.Any())
                {
                    return new ApplicationResponseDTO<IEnumerable<Vendor>>
                    {
                        Data = null,
                        Error = new()
                        {
                            Code = (int)ErrorCodes.NotFound,
                            Message = new List<string> { "No vendors found" }
                        }
                    };
                }

                return new ApplicationResponseDTO<IEnumerable<Vendor>>
                {
                    Data = vendor,
                };
            }
            catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<IEnumerable<Vendor>>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }
        }



        public ApplicationResponseDTO<Vendor> GetVendorById(int vendorId)
        {
            try
            {
                Vendor? vendor = _vendorRepository.GetVendorById(vendorId);
                if(vendor != null)
                {
                    return new ApplicationResponseDTO<Vendor>
                    {
                        Data = vendor
                    };
                }
                else
                {
                    return new ApplicationResponseDTO<Vendor>
                    {
                        Data = null,
                        Message = "Vendor not found in the database"
                    };
                }
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<Vendor>
                {
                    Data = null,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.InternalError,
                        Message = new List<string> { "Internal Db error" },
                    }
                };
            }
        }

        public ApplicationResponseDTO<bool> UpdateVendor(int vendorId, CreateVendorDTO vendorDTO, string jwtToken)
        {
            string currentUser = _tokenService.ExtractUserDetials(jwtToken, "email");
            try
            {
                Vendor? vendor = _vendorRepository.GetVendorById(vendorId);
                if (vendor == null)
                {
                    return new ApplicationResponseDTO<bool>
                    {
                        Data =false,
                        Message = "Vendor not found in the database"
                    };

                }
                using (var transaction = _vendorRepository.BeginTransaction())
                {
                    try
                    {
                        bool res = _vendorRepository.UpdateVendor(vendor, vendorDTO, currentUser);
                        _vendorCategoryRepository.RemoveAllMappingsByVendorId(vendor.Id); // remove all the mapping entries(might add this in a transaction)
                        var entries = vendorDTO.CategoryIds.Select(categoryId => new VendorCategoryMapping
                        {
                            VendorId = vendorId,
                            CategoryId = categoryId,
                            Status = true
                        }).ToList();

                        _vendorCategoryRepository.AddVendorCategoryEntry(entries);
                        transaction.Commit();
                        return new ApplicationResponseDTO<bool>
                        {
                            Data = res,
                        };
                    }catch (Exception ex)
                    {
                        _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                        transaction.Rollback();
                        return new ApplicationResponseDTO<bool>
                        {
                            Data = false,
                            Error = new()
                            {
                                Code = (int)ErrorCodes.InternalError,
                                Message = new List<string> { ex.Message },
                            },
                        };
                    }
                }


            }catch (Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<bool>
                {
                    Data = false,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }




        }

    
        public ApplicationResponseDTO<bool> ToogleVendorStatus(int vendorId)
        {
            try
            {
                Vendor? vendor = _vendorRepository.GetVendorById(vendorId);
                if (vendor == null)
                {
                    return new ApplicationResponseDTO<bool>
                    {
                        Data = false,
                        Message = "Vendor not found in the database"
                    };
                }
                var response = _vendorRepository.ToggleStatus(vendor);
                return new ApplicationResponseDTO<bool>
                {
                    Data = response,
                };
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<bool>
                {
                    Data = false,
                    Error = new()
                    {
                        Code = (int)ErrorCodes.DatabaseError,
                        Message = new List<string> { ex.Message }
                    }
                };
            }



        }

        public ApplicationResponseDTO<VendorFormData> GetFormData()
        {
            try
            {
                IEnumerable<VendorFormTypes> types = _vendorRepository.GetTypesForForm();
                IEnumerable<VendorFormCategories> categories = _categoryRepository.GetCategoriesForForm();

                return new ApplicationResponseDTO<VendorFormData>
                {
                    Data = new VendorFormData()
                    {
                        Categories = categories,
                        VednorTypes = types,
                    }
                };
            }catch(Exception ex)
            {
                _errorLog.LogError((int)ErrorCodes.InternalError, ex);
                return new ApplicationResponseDTO<VendorFormData>
                {
                    Error = new Error
                    {
                        Code = (int)(ErrorCodes.InternalError),
                        Message = new List<string> { ex.Message },
                    }
                };
            }
            
        }
    }
}


