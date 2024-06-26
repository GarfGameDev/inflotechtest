using System;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public ViewResult UserLogs(string searchString, string sortOrder)
        {
            List<UserLog>? userLogs = new List<UserLog>();
            foreach (string? line in System.IO.File.ReadLines(@"./logs/log-20240625.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var deserializedItem = JsonConvert.DeserializeObject<UserLog>(line);
#pragma warning disable CS8604 // Possible null reference argument.
                userLogs.Add(deserializedItem);
#pragma warning restore CS8604 // Possible null reference argument.

                
            }

            ViewBag.DescriptionSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.SearchString = searchString;

            IEnumerable<UserLog> filteredLogs = new List<UserLog>();
            if (!String.IsNullOrEmpty(searchString))
            {
                filteredLogs = userLogs.Where(s => s.Description?.Contains(searchString) == true);

                if (!String.IsNullOrEmpty(sortOrder))
                {
                    return View(SortLogs(sortOrder, filteredLogs));
                }
                return View(filteredLogs);
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                return View(SortLogs(sortOrder, userLogs));
            }
            

            
/*            ViewBag.DescriptionSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            switch (sortOrder)
            {
                case "description_desc":
                   var sortedUsers = filteredLogs.OrderByDescending(s => s.Description);
                    return View(sortedUsers);
                case "description_asc":
                    sortedUsers = filteredLogs.OrderBy(s => s.Description );
                    return View(sortedUsers);
                case "date_desc":
                    sortedUsers = filteredLogs.OrderByDescending(s => s.CreatedAt);
                    return View(sortedUsers);
                case "date_asc":
                    sortedUsers = filteredLogs.OrderBy(s => s.CreatedAt);
                    return View(sortedUsers);
                default:
                    break;
            }*/



            return View(userLogs);
            

        }

        private IEnumerable<UserLog> SortLogs(string sortOrder, IEnumerable<UserLog> userLogs)
        {


            switch (sortOrder)
            {
                case "description_desc":
                    var sortedLogs = userLogs.OrderByDescending(s => s.Description);
                    return sortedLogs;
                case "description_asc":
                    sortedLogs = userLogs.OrderBy(s => s.Description);
                    return sortedLogs;
                case "date_desc":
                    sortedLogs = userLogs.OrderByDescending(s => s.CreatedAt);
                    return sortedLogs;
                case "date_asc":
                    sortedLogs = userLogs.OrderBy(s => s.CreatedAt);
                    return sortedLogs;
                default:
                    return userLogs;
            }
        }
    }
}
