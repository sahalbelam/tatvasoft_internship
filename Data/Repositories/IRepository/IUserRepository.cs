using UserBackend.Models;
using System.Collections.Generic;

namespace UserBackend.Data.Repositories.IRepository;
public interface IUserRepository
{
    void AddUser(User user);
    User GetUserById(int userId);
    User GetRoleById(int RoleId);

    IEnumerable<User> GetAllUsers();
    IEnumerable<User> GetAllUsersOrderedByUsername();
    IEnumerable<object> GetUsersGroupedByRole();
    void SaveChanges();
}