using System;
using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet("{id}")]
    public ViewResult List(int id)
    {
        switch (id)
        {
            case 1:
                return View(FilterUsers(true, false));
            case 2:
                return View(FilterUsers(false, true));
            case 3:
                return View(FilterUsers(false, false));
            default:
                Console.WriteLine("There's been an uninteded issue with filtering users");
                return View(null);
        }

        UserListViewModel FilterUsers(bool showAll, bool isActive)
        {
            var items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive
            });

            if (showAll)
            {
                var model = new UserListViewModel
                {
                    Items = items.ToList()
                };
                return model;
            }
            else
            {
                var model = new UserListViewModel
                {
                    Items = items.Where(p => p.IsActive == isActive).ToList()
                };
                return model;
            }            
        }
    }
}
