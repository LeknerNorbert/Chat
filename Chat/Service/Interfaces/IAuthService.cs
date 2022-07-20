using Chat.Models;

namespace Chat.Logic.Interfaces
{
    public interface IAuthService
    {
        public void Create(User user);
        public User Read(string username);
    }
}
