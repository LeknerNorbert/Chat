using Chat.Models;
using Chat.Models.DTOs;
using Chat.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Service
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _db;

        public UsersService(ApplicationDbContext db)
        {
            _db = db;
        }

        public User ReadUser(string username)
        {
            User? user = _db.Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new ArgumentNullException("User does not exist");
            }

            return user;
        }

        public List<UserDto> ReadUsers(string keyword)
        {
            keyword = keyword.ToLower();

            List<UserDto> users = _db.Users
                .Where(u => u.Username.ToLower().Contains(keyword) ||
                            u.Firstname.ToLower().Contains(keyword) ||
                            u.Middlename.ToLower().Contains(keyword) ||
                            u.Lastname.ToLower().Contains(keyword) ||
                            u.EmailUsername.ToLower().Contains(keyword))
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Middlename = u.Middlename,
                    Email = $"{u.EmailUsername}@{u.EmailHostname}"
                })
                .ToList();

            return users;
        }

        public List<FriendRequest> ReadFriendRequest(string username)
        {
            List<FriendRequest> friendRequests = _db.FriendRequests
                .Where(f => f.Sender.Username == username || f.Receiver.Username == username)
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .ToList();

            return friendRequests;
        }

        public List<FriendRequest> ReadAcceptedFriendRequests(string username)
        {
            List<FriendRequest> friendRequests = _db.FriendRequests
                .Where(f => f.Sender.Username == username || f.Receiver.Username == username && f.Status == RequestStatus.Accepted)
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .ToList();

            return friendRequests;
        }

        // Kiolvassa az adatbázisból azokat a barátkéréseket, amiket a felhasználónak küldtek, de még nem fogadta el őket
        public List<FriendRequest> ReadSentFriendRequest(string username)
        {
            List<FriendRequest> friendRequests = _db.FriendRequests
                .Where(f => f.Receiver.Username == username && f.Status == RequestStatus.Sent)
                .Include(f => f.Sender)
                .ToList();

            return friendRequests;
        }

        public void CreateFriendRequest(string senderUsername, string receiverUsername)
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

        public List<PrivateMessage> ReadPrivateMessages(string firstUsername, string secondUsername)
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
