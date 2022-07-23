using Chat.Models;
using Chat.Models.DTOs;

namespace Chat.Service.Interfaces
{
    public interface IUsersService
    {
        public User ReadUser(string username);
        public List<UserDto> ReadUsers(string keyword);
        public List<FriendRequest> ReadFriendRequest(string username);
        public List<FriendRequest> ReadAcceptedFriendRequests(string username);
        public List<FriendRequest> ReadSentFriendRequest(string username);
        public void CreateFriendRequest(string senderUsername, string receiverUsername);
        public void AcceptFriendRequest(int id);
        public void DeclineFriendRequest(int id);
        public List<PrivateMessage> ReadPrivateMessages(string senderUsername, string receiverUsername);
    }
}
