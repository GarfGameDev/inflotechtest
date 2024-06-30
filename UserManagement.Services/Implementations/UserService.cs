using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services;
internal class UserService : IUserService
{
    private readonly DataContext _dataContext;

    public UserService(DataContext context)
    {
        _dataContext = context;
    }
    public async void CreateUser(User user)
    {
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
    }
    public async void DeleteUser(User user)
    {
        var toDelete = _dataContext.Users.Where(a => a.Id == user.Id).FirstOrDefault();
        if (toDelete != null)
        {
            _dataContext.Users.Remove(toDelete);
            await _dataContext.SaveChangesAsync();
        }
    }
    public User GetUser(int id) => throw new NotImplementedException();
    public Task<List<User>> GetUsers() => throw new NotImplementedException();
    public void SaveUserChanges() => throw new NotImplementedException();
    public void UpdateUser(User actor) => throw new NotImplementedException();
}
