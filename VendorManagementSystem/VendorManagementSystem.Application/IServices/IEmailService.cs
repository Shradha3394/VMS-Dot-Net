using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IServices
{
    public interface IEmailService
    {
        void SendLoginEmail(EmailDetailsDTO emailDetailsDto);
    }
}
