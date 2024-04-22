using Microsoft.EntityFrameworkCore.Storage;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IUserRepository
    {
        IDbContextTransaction BeginTransaction();
        int AddUser(User user);
        User? GetUserByEmail(string email);
        IEnumerable<User> GetUsers();
        bool UpdatePassword(string email, string newPassword);
    }
}