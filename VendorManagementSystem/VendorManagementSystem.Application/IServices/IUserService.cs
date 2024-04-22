using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IUserService
    {
        ApplicationResponseDTO<User> CreateUser(CreateUserDTO userDto, string currentUser);
        ApplicationResponseDTO<TokenDTO> Login(LoginDTO loginDto);
        User CreateSuperAdmin(SuperAdminDTO superAdminDto);
        ApplicationResponseDTO<bool> SetUserPassword(UpdatePasswordDTO updatePasswordDto, string _token);
        ApplicationResponseDTO<string> ValidateToken(string token);
        ApplicationResponseDTO<bool> SendForgetPasswordEmail(string email, string redirectUrl);
        ApplicationResponseDTO<IEnumerable<User>> GetAllUsers();
    }
}
