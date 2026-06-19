namespace PlaylistifyAPI.Domain.Entities
{
	public class Song
	{
		public Guid Id { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string Artist { get; private set; } = string.Empty;
		public TimeSpan Duration { get; private set; }

		// Navigation property for many-to-many relationship
		public ICollection<Playlist> Playlists { get; private set; } = new List<Playlist>();

		public Song(Guid id, string title, string artist, TimeSpan duration)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Title cannot be empty.");
			if (string.IsNullOrWhiteSpace(artist))
				throw new ArgumentException("Artist cannot be empty.");
			if (duration <= TimeSpan.Zero)
				throw new ArgumentException("Duration must be greater than zero.");

			Id = id == Guid.Empty ? Guid.NewGuid() : id;
			Title = title;
			Artist = artist;
			Duration = duration;
		}
	}
}