using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly DataContext _db;

        public UserTokenRepository(DataContext db)
        {
            _db = db;
        }
        public int AddToken(string email, string token)
        {
           UserToken? userToken = _db.UserTokens.Where(ut => ut.Email == email).FirstOrDefault();
            if(userToken == null)
            {
                UserToken newUsetToken = new UserToken
                {
                    Email = email,
                    Token = token
                };
                _db.UserTokens.Add(newUsetToken);
            }
             else
            {
                userToken.Token = token;
            }
            int res = _db.SaveChanges();
            return res;
        }

        public int DeleteToken(string email)
        {
            UserToken? userToken = _db.UserTokens.Where(o => o.Email == email).FirstOrDefault();
            if(userToken == null)
            {
                return 0;
            }
            _db.UserTokens.Remove(userToken);
            _db.SaveChanges();
            return 1;
        }

        public string? GetToken(string email)
        {
             string? token = _db.UserTokens.Where(ut =>ut.Email == email).FirstOrDefault()?.Token;
            return token;
        }
    }
}
