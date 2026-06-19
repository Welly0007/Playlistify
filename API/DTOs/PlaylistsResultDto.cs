namespace API.DTOs
{
	public class PlaylistsResultDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int SongCount { get; set; }
		public TimeSpan TotalDuration { get; set; }
	}
}
