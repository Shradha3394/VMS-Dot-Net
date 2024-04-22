using VendorManagementSystem.Application.Exceptions;

namespace VendorManagementSystem.Application.IServices
{
    public interface IErrorLoggingService
    {
        void LogError(int code, Exception ex);
        void LogError(string message);
    }
}
