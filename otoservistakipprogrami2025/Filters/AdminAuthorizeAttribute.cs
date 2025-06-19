using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace otoservistakipprogrami2025.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var kullaniciAdi = context.HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
