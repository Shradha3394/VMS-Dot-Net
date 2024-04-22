using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Infrastructure.Services
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        public void LogError(int code, Exception ex)
        {   
            string message = $"ErrorCode: {code} ErrorMessage: {ex.Message}; Source:{ex.Source} StackTrace:{ex.StackTrace}";
            this.LogError(message);
        }

        public void LogError(string message)
        {
            string basePath = Directory.GetCurrentDirectory();
            string fullPath = $@"{basePath}\errorlog.txt";
            Console.WriteLine(fullPath);
            DateTime timestamp = DateTime.Now;
            message = $"{timestamp}: {message}";
            try
            {
                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    writer.WriteLine(message);
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error in writing to log file",ex);
            }
        }
    }
}
