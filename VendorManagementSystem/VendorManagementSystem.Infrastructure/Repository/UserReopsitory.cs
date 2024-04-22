using Microsoft.EntityFrameworkCore;
using System.Data;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Models.Models;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Application.IServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class UserReopsitory : IUserRepository
    {
        private readonly DataContext _db;
        private readonly IErrorLoggingService _errorLog;

        public UserReopsitory(DataContext db, IErrorLoggingService errorLog)
        {
            _db = db;
            _errorLog = errorLog;
        }
        public int AddUser(User user)
        {
            _db.Users.Add(user);
            return _db.SaveChanges();
        }

        public User? GetUserByEmail(string email)
        {
            User? user = _db.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _db.Users.Where(u => u.Role == "admin").OrderByDescending(u => u.Id).ToList();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            User? user = _db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return false;
            }
            user.Status = true;
            user.Password = newPassword;
            user.UpdatedBy = user.Email;
            user.UpdatedAt = DateTime.UtcNow;
            int changes = _db.SaveChanges();

            return changes > 0;
        }
    }
}
