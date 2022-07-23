using Chat.Logic.Interfaces;
using Chat.Models;
using Chat.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Chat.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(string username, string password)
        {
            User user;

            try
            {
                user = _userService.Read(username);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("User does not exist in database");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong login details");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("User is invalid");
            }

            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User registeredUser = new User
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Middlename = user.Middlename,
                EmailUsername = user.Email.Split('@')[0],
                EmailHostname = user.Email.Split('@')[1],
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            try
            {
                _userService.Create(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok("Successful registration!");
        }

        private string CreateToken(User user)
        {
            // Az adatok a felhasználóról, amik szerepelnek majd a visszaküldött tokenben
            List<Claim> claims = new()
            {
                new Claim("Username", user.Username),
                new Claim("Firstname", user.Firstname),
                new Claim("Lastname", user.Lastname),
                new Claim("Middlename", user.Middlename),
                new Claim("Email", $"{user.EmailUsername}@{user.EmailHostname}")
            };

            // Kulcs, amiből később generáljuk az aláírást
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value
                ));

            // Aláírás
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}   