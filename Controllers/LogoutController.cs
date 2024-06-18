using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BeaverServerReborn.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LogoutController : Controller
	{
		[HttpGet]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Redirect("/");
		}
	}
}
