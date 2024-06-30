using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IUserService
    {
        public Task<List<User>> GetUsers();
        public User GetUser(int id);
        public void CreateUser(User user);
        public void DeleteUser(User user);
        public void SaveUserChanges();
        public void UpdateUser(User user);
    }
}
