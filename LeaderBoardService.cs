
using Microsoft.EntityFrameworkCore;
using System;

namespace BeaverServerReborn
{
    public class LeaderBoardService : ILeaderBoardService
    {
        private readonly ApplicationContext _context;
        private List<Leader> _leaders = new List<Leader>();



        public LeaderBoardService(ApplicationContext context)
        {
            _context = context;
        }
        public List<Leader> GetLeaderBoard()
        {
            return _leaders;
        }

        public async Task UpdateLeaderBoardAsync()
        {
            _leaders.Clear();
            var topUsers = await _context.Users.OrderByDescending(x => x.Balance).Take(10).ToListAsync();
            foreach (var user in topUsers)
            { 
                _leaders.Add(new Leader(user.Username, user.Balance));
            }
        }
    }
}
