using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface ITokenService
    {
        TokenDTO JwtToken(User user, string type);
        string ExtractUserDetials(string jwtToken, string type);
        bool ValidateToken(string jwtToken);
    }
}