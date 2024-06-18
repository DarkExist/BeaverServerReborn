using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;

namespace BeaverServerReborn.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : Controller
	{
		ApplicationContext _context;

        private readonly ILogger<WsTestController> _logger;

        public LoginController(ApplicationContext context, ILogger<WsTestController> logger)
		{
			_context = context;
            _logger = logger;
        }

		[HttpGet]
		public ActionResult GetLogin()
		{
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/test/test");
            }
            return View();
        }


		[HttpPost]
		public async Task<ActionResult> PostLogin()
		{
			var form = HttpContext.Request.Form;
			if (!form.ContainsKey("username") || !form.ContainsKey("password"))
				return BadRequest("Имя пользователя и/или пароль не установлены");

			string username = form["username"];
			string password = form["password"];

			/*ДОБВИТЬ ШИФРОВАНИЕ*/

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

			if (user == null || password != user.Password)
			{
				return BadRequest("Неверное имя пользователя или пароль");
			}

			var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
			_logger.LogInformation($"Пользователь {username} залогинился");

            return Redirect("/test/test");
		}
	}
}
