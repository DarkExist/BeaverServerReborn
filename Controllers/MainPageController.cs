using Microsoft.AspNetCore.Mvc;

namespace BeaverServerReborn.Controllers
{
    [ApiController]
    [Route("/")]
    public class MainPageController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("/login");
        }
    }
}
