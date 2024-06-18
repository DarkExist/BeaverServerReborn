using System.Numerics;

namespace BeaverServerReborn
{
	public class Upgrade
	{
		public string Name { get; private set; } = String.Empty;
		public string Description { get; private set; } = String.Empty;
		public string AdditionalDescription {  get; private set; } = String.Empty;
		public int Income { get; private set; } = 0;
		public int Price { get; private set; }

        public Upgrade()
        {
        }

        public Upgrade(string name, string description, string additionalDesctiption, int income, int price)
		{
			Name = name;
			Description = description;
            AdditionalDescription = additionalDesctiption;
			Income = income;
            Price = price;
		}
	}
}
