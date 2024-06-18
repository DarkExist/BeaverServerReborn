namespace BeaverServerReborn
{
	public class User
	{
		public string Username { get; private set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public int Balance { get; set; }
		public DateTime CreationDate { get; private set; }
		public DateTime LastEnteredDate { get; set; }

        protected User() { }

        public User(string username, string password, string email)
		{
			Username = username;
			Password = password;
			Email = email;
			Balance = 0;
			CreationDate = DateTime.UtcNow;
			LastEnteredDate = DateTime.UtcNow;
		}

	}
}
