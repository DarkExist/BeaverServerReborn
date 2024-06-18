namespace BeaverServerReborn
{
    public interface ILeaderBoardService
    {
        Task UpdateLeaderBoardAsync();
        List<Leader> GetLeaderBoard();
    }
}
