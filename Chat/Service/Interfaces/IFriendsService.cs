using Chat.Models;
using Chat.Models.DTOs;

namespace Chat.Service.Interfaces
{
    public interface IFriendsService
    {
        public User GetUser(string username);
        public List<UserDto> SearchUsers(string keyword);
        public FriendRequest GetFriendRequest(string senderUsername, string receiverUsername);
        public List<FriendRequest> GetFriendRequests(string username);
        public void SendFriendRequest(string senderUsername, string receiverUsername);
        public void AcceptFriendRequest(int id);
        public void DeclineFriendRequest(int id);
        public List<PrivateMessage> GetPrivateMessages(string senderUsername, string receiverUsername);
    }
}
