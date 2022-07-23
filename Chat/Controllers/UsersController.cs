using Chat.Models;
using Chat.Models.DTOs;
using Chat.Service;
using Chat.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chat.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Search(string keyword)
        {
            List<UserDto> users = _usersService.ReadUsers(keyword);

            return Ok(users); 
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Friends()
        {
            // Kiolvassa a tokenből a kérést küldő felhasználónevét
            string username = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;

            List<FriendRequest> friendRequests = _usersService.ReadAcceptedFriendRequests(username);
            List<UserDto> friends = new();

            foreach(FriendRequest friendRequest in friendRequests)
            {
                if (friendRequest.Sender.Username == username)
                {
                    friends.Add(new UserDto
                    {
                        Username = friendRequest.Receiver.Username,
                        Firstname = friendRequest.Receiver.Firstname,
                        Lastname = friendRequest.Receiver.Lastname,
                        Middlename = friendRequest.Receiver.Middlename,
                        Email = $"{friendRequest.Receiver.EmailUsername}@{friendRequest.Receiver.EmailHostname}"
                    });
                }
                else
                {
                    friends.Add(new UserDto
                    {
                        Username = friendRequest.Sender.Username,
                        Firstname = friendRequest.Sender.Firstname,
                        Lastname = friendRequest.Sender.Lastname,
                        Middlename = friendRequest.Sender.Middlename,
                        Email = $"{friendRequest.Sender.EmailUsername}@{friendRequest.Sender.EmailHostname}"
                    });
                }
            }

            return Ok(friends);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult FriendRequests()
        {
            // Kiolvassa a tokenből a kérést küldő felhasználónevét
            string addresseeUsername = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;

            List<FriendRequest> friendRequests = _usersService.ReadSentFriendRequest(addresseeUsername);
            List<FriendRequestDto> friendRequestDtos = new();

            foreach(FriendRequest friendRequest in friendRequests)
            {
                friendRequestDtos.Add(new FriendRequestDto
                {
                    Id = friendRequest.Id,
                    Sender = new UserDto
                    {
                        Username = friendRequest.Sender.Username,
                        Firstname = friendRequest.Sender.Firstname,
                        Lastname = friendRequest.Sender.Lastname,
                        Middlename = friendRequest.Sender.Middlename,
                        Email = $"{friendRequest.Sender.EmailUsername}@{friendRequest.Sender.EmailHostname}"
                    },
                    Status = friendRequest.Status,
                    Created = friendRequest.Created,
                });
            }

            return Ok(friendRequestDtos);
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SendFriendRequest([FromBody] string addresseeUsername)
        {
            // Kiolvassa a tokenből a kérést küldő felhasználónevét
            string senderUsername = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            List<FriendRequest> friendRequests = _usersService.ReadFriendRequest(senderUsername);

            if (senderUsername == addresseeUsername)
            {
                return BadRequest("You can not send a friend request to yourself");
            }

            // Nem küldhet a felhasználó barátkérést valakinek, aki már küldött neki, vagy már eleve a barátja
            if (friendRequests.FirstOrDefault(f => f.Sender.Username == addresseeUsername || f.Receiver.Username == addresseeUsername) != null)
            {
                return BadRequest("You can not send a friend request to him");
            }

            try
            {
                _usersService.CreateFriendRequest(senderUsername, addresseeUsername);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
                
            }
      
            return Ok("Friend request sent");
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AcceptFriendRequest([FromBody] int Id)
        {
            // Kiolvassa a tokenből a barát kérés címzettjének a felhasználónevét
            string receiverUsername = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            List<FriendRequest> friendRequests = _usersService.ReadSentFriendRequest(receiverUsername);

            // Ha nincs ennek a felhasználónak címzett barátkérés ezzel az azonosítóval, akkor megtagadja a módosítási kérelmet
            if(friendRequests.FirstOrDefault(f => f.Id == Id) == null)
            {
                return BadRequest("You have not permission to accept this friend request");
            }

            _usersService.AcceptFriendRequest(Id);

            return Ok("Friend request accepted");
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeclineFriendRequest([FromBody] int Id)
        {
            // Kiolvassa a tokenből a barát kérés címzettjének a felhasználónevét
            string receiverUsername = User.Claims.FirstOrDefault(c => c.Type == "Username").Value;
            List<FriendRequest> friendRequests = _usersService.ReadSentFriendRequest(receiverUsername);

            // Ha nincs ennek a felhasználónak címzett barátkérés ezzel az azonosítóval, akkor megtagadja a módosítási kérelmet
            if (friendRequests.FirstOrDefault(f => f.Id == Id) == null)
            {
                return BadRequest("You have not permission to decline this friend request");
            }

            _usersService.DeclineFriendRequest(Id);

            return Ok("Friend request denied");
        }
    }
}
