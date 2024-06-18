using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Console;
using System.Text.RegularExpressions;

namespace BeaverServerReborn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        ApplicationContext _context;

        private readonly ILogger<WsTestController> _logger;

        public RegisterController(ApplicationContext context, ILogger<WsTestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private bool UsernameCheck(string username)
        {
            bool flag = true;
            if (username.Length > 12) flag = false;
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$")) flag = false;
            return flag;
        }

        private bool PasswordCheck(string password)
        {
            bool flag = true;
            if (password.Length < 6 || password.Length > 52) flag = false;
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9_!@#$%^&*()+]+$")) flag = false;
            return flag;
        }

        [HttpGet]
        public ActionResult GetRegister()
        {
            return View();
        }


        [HttpPost]
        public ActionResult PostRegister()
        {
            var form = HttpContext.Request.Form;
            if (!(form.ContainsKey("username")) || !(form.ContainsKey("password")))
                return BadRequest("Имя пользователя и/или пароль не установлены");

            string username = form["username"];
            string password = form["password"];
            string repeatPassword = form["repeatPassword"];

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null) return BadRequest("Такое имя уже занято");

            if (!UsernameCheck(username))
                return BadRequest("Такое имя недопустимо");

            if (!PasswordCheck(password)) return BadRequest("Такой пароль недопустим");

            if (password != repeatPassword)
                return BadRequest("Поля 'Пароль' и 'Подтверждение пароля' не совпадают");



            

            var newUser = new User(username, password, "old");
            _context.Users.Add(newUser);
            _context.SaveChanges();
            _logger.LogInformation($"Пользователь {username} успешно создан");
            return Ok("Пользователь успешно создан");
        }
    }
}
