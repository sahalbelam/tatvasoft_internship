// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApi.Data.Repositories.IRepository;
using UserRegistrationApi.Models;
using UserRegistrationApi.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace UserRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private static readonly List<User> _usersInMemory = new List<User>(); // Static list to hold in-memory users
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            // Check if userId already exists
            var existingUserById = _userRepository.GetUserById(user.UserId);
            if (existingUserById != null)
            {
                return BadRequest("User ID already exists.");
            }

            // Check if username already exists
            var existingUserByUsername = _userRepository.GetAllUsers().FirstOrDefault(u => u.Username == user.Username);
            if (existingUserByUsername != null)
            {
                return BadRequest("Username already exists.");
            }

            // Check if roleId exists
            var roleExists = _userRepository.GetRoleById(user.RoleId);
            if (roleExists != null)
            {
                return BadRequest("Role ID already exists.");
            }

            // Register the user
            _userRepository.AddUser(user);
            _userRepository.SaveChanges();
            _usersInMemory.Add(user);

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { message = "User registered successfully!", token });
        }
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User") // You can set roles dynamically if needed
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Username is not exist.");
            }

            if (user.Password != loginDto.Password)
            {
                return Unauthorized("Invalid Password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { message = "User Login successfully!", token });

        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Password = user.Password,
                RoleId = user.RoleId,
                Role = user.Role != null ? new RoleDto
                {
                    RoleId = user.Role.RoleId,
                    RoleName = user.Role.RoleName,
                    Users = user.Role.Users.ToList()
                } : null
            };

            return Ok(userDto);
        }
        [HttpGet("all-in-memory")]
        public IActionResult GetAllUsersInMemory()
        {
            return Ok(_usersInMemory.ToList());
        }

        [HttpGet("all-from-database")]
        public IActionResult GetAllUsersFromDatabase()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("ordered-by-username")]
        public IActionResult GetUsersOrderedByUsername()
        {
            var users = _userRepository.GetAllUsersOrderedByUsername();
            return Ok(users);
        }

        [HttpGet("grouped-by-role")]
        public IActionResult GetUsersGroupedByRole()
        {
            var usersGroupedByRole = _userRepository.GetUsersGroupedByRole();
            return Ok(usersGroupedByRole);
        }
    }
}
