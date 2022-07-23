using Chat.Logic.Interfaces;
using Chat.Models;
using Chat.Models.DTOs;

namespace Chat.Logic
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User Read(string username)
        {
            User? user = _db.Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new InvalidOperationException("User does not exist");
            }

            return user;
        }
    }
}
