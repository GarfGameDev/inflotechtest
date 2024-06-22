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

    [HttpGet("{filter}")]
    public ViewResult List(string filter)
    {
        switch (filter)
        {
            case "showall":
                return View(FilterUsers(true, false));
            case "active":
                return View(FilterUsers(false, true));
            case "notactive":
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
                IsActive = p.IsActive,
                DateOfBirth = p.DateOfBirth
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

    [HttpGet("{id}/Details")]
    public ViewResult Details(int id)
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive
        }).Where(p => p.Id == id);

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
        
    }
}
