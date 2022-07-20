using Chat.Models;
using Chat.Models.DTOs;
using Chat.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Service
{
    public class FriendsService : IFriendsService
    {
        private readonly ApplicationDbContext _db;

        public FriendsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public User GetUser(string username)
        {
            User? user = _db.Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentNullException("User does not exist");
            }

            return user;
        }

        public List<UserDto> SearchUsers(string keyword)
        {
            List<UserDto> users = _db.Users
                .Where(u => u.Username.Contains(keyword) &&
                            u.Firstname.Contains(keyword) &&
                            u.Middlename.Contains(keyword) &&
                            u.Lastname.Contains(keyword) &&
                            u.Email.Contains(keyword))
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Middlename = u.Middlename,
                    Email = u.Email
                })
                .ToList();

            return users;
        }

        public FriendRequest GetFriendRequest(string senderUsername, string receiverUsername)
        {
            FriendRequest? friendRequest = _db.FriendRequests
                .FirstOrDefault(f => f.Sender.Username == senderUsername && f.Receiver.Username == receiverUsername);

            if (friendRequest == null)
            {
                throw new ArgumentNullException("Friend request does not exist");
            }

            return friendRequest;
        }

        public List<FriendRequest> GetFriendRequests(string username)
        {
            List<FriendRequest> friendRequests = _db.FriendRequests
                .Where(f => f.Sender.Username == username && f.Receiver.Username == username)
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .ToList();

            return friendRequests;
        }

        public void SendFriendRequest(string senderUsername, string receiverUsername)
        {
            User? sender = _db.Users
                .FirstOrDefault(u => u.Username == senderUsername);

            User? receiver = _db.Users
                .FirstOrDefault(u => u.Username == receiverUsername);

            if (sender == null || receiver == null)
            {
                throw new ArgumentNullException("Sender or receiver does not exist");
            }

            FriendRequest friendRequest = new FriendRequest
            {
                Sender = sender,
                Receiver = receiver,
                Status = RequestStatus.Sent,
                Created = DateTime.Now
            };

            _db.FriendRequests.Add(friendRequest);
            _db.SaveChanges();
        }

        public void AcceptFriendRequest(int id)
        {
            FriendRequest? friendRequest = _db.FriendRequests
                .FirstOrDefault(f => f.Id == id);

            if (friendRequest == null)
            {
                throw new ArgumentNullException("FriendRequest does not exist");
            }

            friendRequest.Status = RequestStatus.Accepted;
            _db.SaveChanges();
        }

        public void DeclineFriendRequest(int id)
        {
            FriendRequest? friendRequest = _db.FriendRequests
                .FirstOrDefault(f => f.Id == id);

            if (friendRequest == null)
            {
                throw new ArgumentNullException("FriendRequest does not exist");
            }

            friendRequest.Status = RequestStatus.Denied;
            _db.SaveChanges();
        }

        public List<PrivateMessage> GetPrivateMessages(string firstUsername, string secondUsername)
        {
            List<PrivateMessage> privateMessages = _db.PrivateMessages
                .Where(p => (p.Sender.Username == firstUsername && p.Sender.Username == secondUsername) || 
                            (p.Receiver.Username == firstUsername && p.Receiver.Username == secondUsername))
                .OrderByDescending(p => p.Created)
                .ToList();

            return privateMessages;
        }
    }
}
