// UserRepository.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UserRegistrationApi.Data.Repositories.IRepository;
using UserRegistrationApi.Models;

namespace UserRegistrationApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRegistrationContext _context;

        public UserRepository(UserRegistrationContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                   .Include(u => u.Role) // Assuming navigation property is named 'Role'
                   .FirstOrDefault(u => u.UserId == userId);
        }

        public User GetRoleById(int RoleId)
        {
            return _context.Users
                   .Include(u => u.Role) // Assuming navigation property is named 'Role'
                   .FirstOrDefault(u => u.RoleId == RoleId);
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
            return _context.Users
                .Include(u => u.Role)
                .GroupBy(u => u.Role.RoleName)
                .Select(g => new { Role = g.Key, Users = g.ToList() })
                .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
