namespace BeaverServerReborn
{
    public class PlayerUpgrade
    {
        public int Id {  get; private set; }
        public string Username { get; private set; }
        public string UpgradeName { get; private set; }
        public int Count { get; set; }

        protected PlayerUpgrade() { }

        public PlayerUpgrade(string username, string upgradeName, int count)
        {
            Username = username;
            UpgradeName = upgradeName;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Username} {UpgradeName} {Count}";
        }
    }
}
