using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Models;

namespace otoservistakipprogrami2025.Controllers
{
    public class DebugController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DebugController> _logger;
        private readonly OtoServisDbContext _context;

        public DebugController(
            IConfiguration configuration,
            ILogger<DebugController> logger,
            OtoServisDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginFlow()
        {
            try
            {
                var debugInfo = new
                {
                    // Session bilgileri (Identity yerine Session kullanıyorsunuz)
                    HasSessionUserId = HttpContext.Session.GetString("UserId") != null,
                    SessionUserId = HttpContext.Session.GetString("UserId"),
                    SessionUserEmail = HttpContext.Session.GetString("UserEmail"),

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

                    // Session bilgileri
                    SessionId = HttpContext.Session.Id,
                    SessionKeys = HttpContext.Session.Keys.ToList()
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
                    // Kendi tablolarınızı kontrol edin
                    tableInfo.Add("Users", await _context.Users.CountAsync());
                    // Başka tablolarınız varsa ekleyin
                    // tableInfo.Add("Araclar", await _context.Araclar.CountAsync());
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

                // Kendi User modelinizle kullanıcı bulma
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    return Json(new { Success = false, Message = "User not found" });
                }

                // Basit şifre kontrolü (gerçek uygulamada hash kullanın)
                bool passwordMatch = user.Password == model.Password;

                var loginResult = new
                {
                    Success = passwordMatch,
                    UserFound = true,
                    User = new
                    {
                        user.UserId,
                        user.Email,
                        user.Ad,
                        user.KayitTarihi
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