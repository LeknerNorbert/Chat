using Chat.Models;
using Chat.Models.DTOs;
using Chat.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly FriendsService _friendsService;

        public FriendsController()
        {
            // service init here
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Search(string keyword)
        {
            List<UserDto> users = _friendsService.SearchUsers(keyword);

            return Ok(users); 
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Friends(string username)
        {
            List<FriendRequest> friendRequests = new List<FriendRequest>(20);
            List<User> friends = new List<User>(20);

            try
            {
                friendRequests = _friendsService.GetFriendRequests(username);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            foreach(FriendRequest friendRequest in friendRequests)
            {
                if (friendRequest.Sender.Username == username)
                {
                    friends.Add(friendRequest.Sender);
                }
                else
                {
                    friends.Add(friendRequest.Receiver);
                }
            }

            return Ok(friends);
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> FriendRequests()
        {
            throw new NotImplementedException();
        }

        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Messages()
        {
            throw new NotImplementedException();
        }
    }
}
