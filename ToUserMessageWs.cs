namespace BeaverServerReborn
{
    public class ToUserMessageWs
    {
        public int Balance { get; set; }
        public Dictionary<string, int>? UserUpgrades { get; set;}
        public List<Upgrade>? AllUpgrades { get; set;}


        public ToUserMessageWs(int balance)
        {
            Balance = balance;
        }

        public ToUserMessageWs(int balance, Dictionary<string, int> userUpgrades)
        {
            Balance = balance;
            UserUpgrades = userUpgrades;
        }


        public ToUserMessageWs(int balance, Dictionary<string, int> userUpgrades, 
            List<Upgrade> allUpgrades)
        {
            Balance = balance;
            UserUpgrades = userUpgrades;
            AllUpgrades = allUpgrades;
        }
    }
}
