using GED_APP.Data;
using GED_APP.Models;
using GED_APP.Repository.Interfaces;
using System.Security.Cryptography;

namespace GED_APP.Repository.Implementations
{
    public class UserRepo : IUser
    {
        private readonly AppDbContext _context;
        private IConfiguration _config;
        public UserRepo(AppDbContext appDbContext, IConfiguration configuration)
        {
            _context = appDbContext;
            _config = configuration;

        }
        public int Add(User user)
        {
            int res = -1;
            if (user == null)
            {
                res = 0;
            }
            else
            {
                // hashing password
                var salBytes = new byte[64];
                var provider = new RNGCryptoServiceProvider();
                provider.GetNonZeroBytes(salBytes);
                var salt = Convert.ToBase64String(salBytes);
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(user.Password, salBytes, 1000);
                var hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
                //fin hashing pass

                user.Password = hash;
                user.SaltPassword = salt;
                _context.Users.Add(user);
                _context.SaveChanges();
                res = user.Id;
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                res = 0;
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                res = user.Id;
            }
            return res;

        }


        public User AuthenticateUser(User usr)
        {


            var user = _context.Users.SingleOrDefault(u => u.UserEmail == usr.UserEmail && u.Password == usr.Password);
            // if not user exist
            if (user == null)
                return null;
            return user;


        }
        //public string GenerateToken(User usr)
        //{
        //    // Generate Token if success authentifaction
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Audience"], null,
        //        expires: DateTime.Now.AddMinutes(1),
        //        signingCredentials: credentials);
        //    usr.Token = new JwtSecurityTokenHandler().WriteToken(token);
        //    _context.Users.Update(usr);
        //    _context.SaveChanges();
        //    return usr.Token;


        //}


        public int Update(User user)
        {
            int res = -1;
            var u = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            if (u == null)
            {
                res = 0;
            }
            else
            {
                u.UserEmail = user.UserEmail;
                u.UserName = user.UserName;
                u.Password = user.Password;
                u.Service=user.Service;
                u.Role = user.Role;
                _context.Users.Update(u);
                _context.SaveChanges();
                res = u.Id;
            }
            return res;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

        public ICollection<User> GetAll()
        {
            //var list = _context.Users.ToList();
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }

        public string HashPassword(string password)
        {
            var salBytes = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(salBytes);
            var salt = Convert.ToBase64String(salBytes);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salBytes, 1000);
            var hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return hash;
        }

        public User GetByEmail(string email)
        {
            var user = _context.Users.Where(u => u.UserEmail == email).FirstOrDefault();
            return user;
        }
    }
}
