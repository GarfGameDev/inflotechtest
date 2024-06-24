using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Web.Models;

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
            _logger.LogInformation("About page visited at {}",
                DateTime.UtcNow.ToLongTimeString());
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
/*            string json = System.IO.File.ReadAllText(@"./logs/log-20240623.json");
            var items = Newtonsoft.Json.JsonConvert.DeserializeObject(json);*/
            _logger.LogInformation(UserLogging.GetItem, "Getting item {Id}", id);
            foreach (string line in System.IO.File.ReadLines(@"./logs/log-20240623.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                UserLog? userLog = JsonConvert.DeserializeObject<UserLog>(line);
            }

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
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
          return _context.Users.Any(e => e.Id == id);
        }
    }
}
