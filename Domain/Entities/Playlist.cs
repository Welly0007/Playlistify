namespace PlaylistifyAPI.Domain.Entities
{
	public class Playlist
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public string Description { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public int TotalSongs => _songs.Count;
		public TimeSpan TotalDuration => TimeSpan.FromTicks(_songs.Sum(s => s.Duration.Ticks));

		// navigation property for songs, many to many
		private readonly List<Song> _songs = new();
		public IReadOnlyCollection<Song> Songs => _songs.AsReadOnly();

		public Playlist(string name, string description)
		{
			Id = Guid.NewGuid();
			Name = name;
			CreatedAt = DateTime.UtcNow;
			Description = description ?? string.Empty;
		}

		public void AddSong(Song song)
		{
			if (song == null)
				throw new ArgumentNullException(nameof(song));

			_songs.Add(song);
		}

		public void RemoveSong(Song song)
		{
			if (song == null)
				throw new ArgumentNullException(nameof(song));

			_songs.Remove(song);
		}

		public void Update(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Playlist name cannot be empty.");

			Name = name;
			Description = description ?? string.Empty;
		}
	}
}