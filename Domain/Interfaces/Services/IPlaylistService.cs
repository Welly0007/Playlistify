using PlaylistifyAPI.Domain.Entities;

namespace Domain.Interfaces.Services
{
	public interface IPlaylistService
	{
		IQueryable<Playlist> GetPlaylistsQuery();
		public Task<Playlist?> GetPlaylistByIdAsync(Guid id);
		public Task<Playlist> CreatePlaylistAsync(string name, string description);
		public Task AddSongToPlaylistAsync(Guid playlistId, Guid songId);
		public Task RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId);
		public Task UpdatePlaylistAsync(Guid id, string name, string description);
		public Task DeletePlaylistAsync(Guid id);
		Task<IEnumerable<Song>> GetAllSongsAsync();
		Task<Song> CreateSongAsync(string title, string artist, TimeSpan duration);
	}
}
