using System.Numerics;

namespace BeaverServerReborn
{
	public interface IUpgrade
	{
		public string Name { get; }
		public string Description { get; }
		public BigInteger Price { get; }
	}
}
