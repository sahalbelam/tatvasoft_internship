// UserRepository.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UserBackend.Data.Repositories.IRepository;
using UserBackend.Models;
using Microsoft.Extensions.Logging;

namespace UserBackend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRegistrationContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserRegistrationContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                _logger.LogError("Attempted to add a null user.");
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
            }

            return user;
        }

        public User GetRoleById(int roleId)
        {
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.RoleId == roleId);

            if (user == null)
            {
                _logger.LogWarning($"Role with ID {roleId} not found.");
            }

            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                           .Include(u => u.Role)
                           .ToList();
        }

        public IEnumerable<User> GetAllUsersOrderedByUsername()
        {
            return _context.Users
                           .Include(u => u.Role)
                           .OrderBy(u => u.Username)
                           .ToList();
        }

        public IEnumerable<object> GetUsersGroupedByRole()
        {
            var groupedUsers = _context.Users
                                       .Include(u => u.Role)
                                       .GroupBy(u => u.Role.RoleName)
                                       .Select(g => new { Role = g.Key, Users = g.ToList() })
                                       .ToList();

            if (!groupedUsers.Any())
            {
                _logger.LogWarning("No users found to group by role.");
            }

            return groupedUsers;
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes.");
                throw;
            }
        }
    }
}
