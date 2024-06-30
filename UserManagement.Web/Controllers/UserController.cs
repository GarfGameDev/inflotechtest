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
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataContext _context;

        public UserController(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: User/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> Index()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("showactive")]
        public async Task<ActionResult<IEnumerable<User>>> FilterActive(bool showActive)
        {
            if (showActive)
            {
                return await _context.Users.Where(p => p.IsActive == true).ToListAsync();
            }
            else
            {
                return await _context.Users.Where(p => p.IsActive == false).ToListAsync();
            }
        }

        // GET: User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Details(long? id)
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
            

            return user;
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        public async Task<ActionResult<User>> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation(UserLogging.InsertItem, "User has been created");
            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }

        // PUT: User/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation(UserLogging.UpdateItem, "User has been updated");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: User/5
        [HttpDelete("{id}")]
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
            return NoContent();
        }

        private bool UserExists(long id)
        {
          return _context.Users.Any(e => e.Id == id);
        }

        // GET user/userlog/id
        [HttpGet("userlog/{id}")]
        public List<UserLog> GetUserLogs(long? id)
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

        [HttpGet("logs")]
        public List<UserLog> UserLogs()
        {
            List<UserLog>? userLogs = new List<UserLog>();
            foreach (string? line in System.IO.File.ReadLines(@"./logs/log-20240625.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var deserializedItem = JsonConvert.DeserializeObject<UserLog>(line);
#pragma warning disable CS8604 // Possible null reference argument.
                userLogs.Add(deserializedItem);
#pragma warning restore CS8604 // Possible null reference argument.           
            }

            return userLogs;
        }
        // GET user/logs/id
        [HttpGet("logs/{createdAt}")]
        public UserLog LogDetails(string createdAt)
        {
            UserLog? userLog = new UserLog();
            foreach (string? line in System.IO.File.ReadLines(@"./logs/log-20240625.json").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var deserializedItem = JsonConvert.DeserializeObject<UserLog>(line);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (deserializedItem.CreatedAt == createdAt)
                {
                    userLog = deserializedItem;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            DateTime date;
            if (DateTime.TryParse(userLog.CreatedAt, out date))
            {
                userLog.CreatedAt = date.ToString("MMMM dd, yyyy, H:mm:ss");
            }
            return userLog;
        }
/*        private IEnumerable<UserLog> SortLogs(string sortOrder, IEnumerable<UserLog> userLogs)
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
        }*/
    }
}
