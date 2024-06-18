using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeaverServerReborn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaderBoardController : Controller
    {
        private readonly ApplicationContext _context;
        private List<Leader> _leaders = new List<Leader>();

        public LeaderBoardController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Leader>> GetLeaderBoard()
        {
            _leaders.Clear();
            var topUsers = await _context.Users.OrderByDescending(x => x.Balance).Take(10).ToListAsync();
            foreach (var user in topUsers)
            {
                _leaders.Add(new Leader(user.Username, user.Balance));
            }
            return _leaders;
        }
    }
}
