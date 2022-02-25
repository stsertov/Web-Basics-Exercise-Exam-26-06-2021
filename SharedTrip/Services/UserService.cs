using SharedTrip.Contracts;
using SharedTrip.Data.Common;
using SharedTrip.Data.Models;
using SharedTrip.Models.Users;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharedTrip.Services
{
    public class UserService : IUserService
    {
        private IRepository repository;
        public UserService(IRepository repository)
        {
           this.repository = repository;
        }

        public void RegisterUser(UserRegisterFormModel model)
        {
            User user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                Password = HashPassword(model.Password)
            };

            repository.Add<User>(user);
            repository.SaveChanges();
        }

        public string GetUserId(UserLogInFormModel model)
            => repository.All<User>().FirstOrDefault(u => 
                      u.Username == model.Username &&
                      u.Password == HashPassword(model.Password)).Id;

        public bool UserExists(UserLogInFormModel model)
            => repository.All<User>().Any(u => 
                      u.Username == model.Username && 
                      u.Password == HashPassword(model.Password));

        public bool UsernameExists(string username)
            => repository.All<User>().Any(x => 
                      x.Username == username);

        public bool EmailExists(string email)
            => repository.All<User>().Any(x => 
                     x.Email == email);

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
