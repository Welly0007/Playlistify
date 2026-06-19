namespace API.DTOs
{
	public class SongResultDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Artist { get; set; } = string.Empty;
		public TimeSpan Duration { get; set; }
	}
}
