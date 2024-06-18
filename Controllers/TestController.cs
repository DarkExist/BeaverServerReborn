using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeaverServerReborn.Controllers
{
    [Route("/test/test")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly ApplicationContext _context;
        /*private readonly ILeaderBoardService _leaders;*/
        public TestController(ApplicationContext context)
        {
            _context = context;
            /*_leaders = leaders;*/

        }

        [HttpGet]
        public ActionResult GetTest()
        {
            return View();
        }

        /*[HttpGet("/123")]
        public List<Leader> GetTest3()
        {
            return _leaders.GetLeaderBoard();
        }*/

        /*[HttpGet("/asd1337228")]
        public ActionResult GetTest2()
        {
            List<Upgrade> upgrades = new List<Upgrade>();

            upgrades.Add(new Upgrade("Beaver", "Elderly rodent", "Brings 1 logs per second", 1, 100));
            upgrades.Add(new Upgrade("Beaver cabin", "Beaver family cabin", "Brings 4 logs per second", 4, 1100));
            upgrades.Add(new Upgrade("Beaver farm", "Tree farm", "Brings 15 logs per second", 15, 12000));
            upgrades.Add(new Upgrade("Beaver Steamboat", "Steamboat floating on the river and collecting logs", "Brings 65 logs per second", 65, 130000));
            upgrades.Add(new Upgrade("Beaver sawmill", "Beaver log production", "Brings 260 logs per second", 260, 1400000));
            upgrades.Add(new Upgrade("Beaver laboratory", "Log synthesis laboratory", "Brings 1,050 logs per second", 1050, 15000000));
            upgrades.Add(new Upgrade("Beaver temple", "Ancient temple full of logs", "Brings 4100 logs per second", 4100, 160000000));
            upgrades.Add(new Upgrade("Beaver generator", "Generator producing logs from nothing", "Brings 16340 logs per second", 16340, 1700000000));

            foreach (Upgrade upgrade in upgrades)
            {
                _context.Upgrades.Add(upgrade);
            }
            _context.SaveChanges();
            return Ok();
        }*/
    }
}
