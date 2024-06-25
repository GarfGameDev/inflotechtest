using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataContext _context;

        public UserController(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> FilterActive(bool showActive)
        {
            if (showActive)
            {
                return View(await _context.Users.Where(p => p.IsActive == true).ToListAsync());
            }
            else
            {
                return View(await _context.Users.Where(p => p.IsActive == false).ToListAsync());
            }
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(long? id)
        {


            if (id == null || _context.Users == null)
            {
                _logger.LogWarning(UserLogging.GetItemNotFound, "Get({Id}) NOT FOUND", id);
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                _logger.LogWarning(UserLogging.GetItemNotFound, "Get({Id}) NOT FOUND", id);
                return NotFound();
            }

            _logger.LogInformation(UserLogging.GetItem, "User details have been accessed");

            user.UserLogs = GetUserLogs(id);
            

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Forename,Surname,Email,IsActive,DateOfBirth")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation(UserLogging.InsertItem, "User has been created");
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Forename,Surname,Email,IsActive,DateOfBirth")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation(UserLogging.UpdateItem, "User has been updated");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _logger.LogInformation(UserLogging.DeleteItem, "User has been deleted");
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
          return _context.Users.Any(e => e.Id == id);
        }

        private List<UserLog> GetUserLogs(long? id)
        {
            List<UserLog>? userLogs = new List<UserLog>();
            foreach (string? line in System.IO.File.ReadLines(@"./logs/log-20240625.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var deserializedItem = JsonConvert.DeserializeObject<UserLog>(line);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (deserializedItem.Id == id)
                {
                    userLogs.Add(deserializedItem);
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            return userLogs;
            
        }

        public ViewResult UserLogs(string searchString)
        {
            List<UserLog>? userLogs = new List<UserLog>();
            foreach (string? line in System.IO.File.ReadLines(@"./logs/log-20240625.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var deserializedItem = JsonConvert.DeserializeObject<UserLog>(line);
#pragma warning disable CS8604 // Possible null reference argument.
                userLogs.Add(deserializedItem);
#pragma warning restore CS8604 // Possible null reference argument.

                
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                var filteredUsers = userLogs.Where(s => s.Description?.Contains(searchString) == true);
                return View(filteredUsers);
            }

            return View(userLogs);
            

        }
    }
}
