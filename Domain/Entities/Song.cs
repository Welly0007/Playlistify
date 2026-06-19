namespace PlaylistifyAPI.Domain.Entities
{
	public class Song
	{
		public Guid Id { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string Artist { get; private set; } = string.Empty;
		public TimeSpan Duration { get; private set; }

		// navigation property for with playlist, many to many
		public ICollection<Playlist> Playlists { get; private set; } = new List<Playlist>();

		private Song() { }

		public Song(Guid id, string title, string artist, TimeSpan duration)
		{
			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be empty.") : title;
			Artist = string.IsNullOrWhiteSpace(artist) ? throw new ArgumentException("Artist cannot be empty.") : artist;
			Duration = duration;
		}
	}
}