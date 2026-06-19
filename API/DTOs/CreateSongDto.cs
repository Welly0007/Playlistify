namespace API.DTOs
{
	public class CreateSongDto
	{
		public required string Title { get; set; }
		public required string Artist { get; set; }
		public required TimeSpan Duration { get; set; }
	}
}
