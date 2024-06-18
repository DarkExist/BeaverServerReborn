namespace BeaverServerReborn
{
    public class Leader
    {
        public string Name { get; set; } = String.Empty;
        public int Balance { get; set; }
        public int Income { get; set; }

        public Leader(string name, int balance, int income = 0)
        {
            Name = name;
            Balance = balance;
            Income = income;
        }
    }
}
