using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;

namespace YourProjectName.Controllers // Proje isminizle değiştirin
{
    public class DebugController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DebugController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly DbContext _context; // DbContext'inizin tipini yazın (örnek: ApplicationDbContext)

        public DebugController(
            IConfiguration configuration,
            ILogger<DebugController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            DbContext context) // DbContext tipinizi yazın
        {
            _configuration = configuration;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginFlow()
        {
            try
            {
                var debugInfo = new
                {
                    // Authentication bilgileri
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    UserName = User.Identity.Name,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),

                    // Claims bilgileri
                    Claims = User.Claims.Select(c => new {
                        Type = c.Type,
                        Value = c.Value
                    }).ToList(),

                    // Connection String kontrolü
                    HasConnectionString = !string.IsNullOrEmpty(_configuration.GetConnectionString("DefaultConnection")),
                    ConnectionStringPrefix = _configuration.GetConnectionString("DefaultConnection")?.Substring(0, Math.Min(50, _configuration.GetConnectionString("DefaultConnection").Length)) + "...",

                    // Environment bilgileri
                    Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    MachineName = System.Environment.MachineName,

                    // Request bilgileri
                    RequestPath = Request.Path,
                    RequestMethod = Request.Method,
                    UserAgent = Request.Headers["User-Agent"].ToString(),

                    // Cookie bilgileri
                    Cookies = Request.Cookies.Keys.ToList(),

                    // SignIn Manager durumu
                    SignInManagerType = _signInManager.GetType().Name,
                    UserManagerType = _userManager.GetType().Name
                };

                _logger.LogInformation("Debug endpoint called: {@DebugInfo}", debugInfo);

                return Json(debugInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Debug endpoint error");
                return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Database()
        {
            try
            {
                // Database bağlantısını test et
                var canConnect = await _context.Database.CanConnectAsync();

                // Tablo sayısını al (eğer bağlantı varsa)
                var tableInfo = new Dictionary<string, object>();

                if (canConnect)
                {
                    // Bu kısmı kendi DbSet'lerinize göre düzenleyin
                    /*
                    tableInfo.Add("Users", await _context.Users.CountAsync());
                    tableInfo.Add("Roles", await _context.Roles.CountAsync());
                    // Diğer tablolarınızı ekleyin
                    */
                }

                var dbInfo = new
                {
                    CanConnect = canConnect,
                    DatabaseName = _context.Database.GetDbConnection().Database,
                    ConnectionState = _context.Database.GetDbConnection().State.ToString(),
                    ProviderName = _context.Database.ProviderName,
                    TableCounts = tableInfo,
                    PendingMigrations = await _context.Database.GetPendingMigrationsAsync(),
                    AppliedMigrations = await _context.Database.GetAppliedMigrationsAsync()
                };

                _logger.LogInformation("Database debug info: {@DbInfo}", dbInfo);

                return Json(dbInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database debug error");
                return Json(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        public IActionResult TestRedirect()
        {
            try
            {
                _logger.LogInformation("Test redirect called");

                // Farklı redirect yöntemlerini test et
                var redirectTests = new
                {
                    CanRedirectToHome = Url.Action("Index", "Home"),
                    CanRedirectToAdmin = Url.Action("Index", "Admin"),
                    IsLocalUrl_Home = Url.IsLocalUrl("/Home/Index"),
                    IsLocalUrl_Admin = Url.IsLocalUrl("/Admin/Index"),
                    BaseUrl = $"{Request.Scheme}://{Request.Host}",
                    CurrentUrl = Request.GetDisplayUrl()
                };

                return Json(redirectTests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redirect test error");
                return Json(new { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TestLogin([FromBody] TestLoginModel model)
        {
            try
            {
                _logger.LogInformation("Test login attempt for: {Email}", model.Email);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Json(new { Success = false, Message = "User not found" });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                var loginResult = new
                {
                    Success = result.Succeeded,
                    IsLockedOut = result.IsLockedOut,
                    RequiresTwoFactor = result.RequiresTwoFactor,
                    IsNotAllowed = result.IsNotAllowed,
                    User = new
                    {
                        user.Id,
                        user.Email,
                        user.EmailConfirmed,
                        user.LockoutEnabled,
                        LockoutEnd = user.LockoutEnd?.ToString()
                    }
                };

                _logger.LogInformation("Test login result: {@LoginResult}", loginResult);

                return Json(loginResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test login error");
                return Json(new { Error = ex.Message });
            }
        }
    }

    public class TestLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}