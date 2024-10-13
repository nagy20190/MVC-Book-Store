using BKStore_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BKStore_MVC.Controllers
{
    public class CookieController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CookieController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        //Cookie/DecodeIdentityCookie
        public async Task<IActionResult> DecodeIdentityCookie()
        {
            var cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            if (cookie != null)
            {
                var ticket = await _signInManager.Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
                if (ticket != null)
                {
                    var userId = ticket.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    return Content($"User ID from cookie: {userId}");
                }
            }
            return Content("Identity cookie not found or invalid.");
        }

        public IActionResult AddDeliveryIDCookie(string UserID)
        {
            string DeliveryIDValue = UserID;
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append("Did", DeliveryIDValue, options);

            return Content($"New cookie 'Did' added with value: {DeliveryIDValue}");
        }
    }
}
