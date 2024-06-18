
namespace BeaverServerReborn
{
    public class LeaderBoardUpdateService : BackgroundService
    {
        private readonly ILeaderBoardService _leaderboardService;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(5);


        public LeaderBoardUpdateService(ILeaderBoardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _leaderboardService.UpdateLeaderBoardAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }
    }
}
