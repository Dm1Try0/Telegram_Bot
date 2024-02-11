namespace Ref.Bot.Business.Dtos
{
	public class UserDto
	{
		public long Id { get; set; }
		public long TelegramId { get; set; }
		public DateTime StartingTime { get; set; }
		public string Role { get; set; }
		public long Status { get; set; }
		public string Username { get; set; }
		public string First { get; set; }
		public DateTime LastMessage { get; set; }
		public long SpamControl { get; set; }
		public string HimReferal { get; set; }
		public bool IsReferal { get; set; }
		public long AnketAmount { get; set; }
	}
}
