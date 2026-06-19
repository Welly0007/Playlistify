using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using PlaylistifyAPI.Domain.Entities;

namespace Infrastructure.Services
{
	public class PlaylistService : IPlaylistService
	{
		private readonly IPlaylistRepository _playlistRepo;
		private readonly ISongRepository _songRepo;

		public PlaylistService(IPlaylistRepository playlistRepo, ISongRepository songRepo)
		{
			_playlistRepo = playlistRepo;
			_songRepo = songRepo;
		}
		// Crud operations for playlists
		public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync()
		{
			return await _playlistRepo.GetAllAsync();
		}

		public async Task<Playlist?> GetPlaylistByIdAsync(Guid id)
		{
			return await _playlistRepo.GetPlaylistWithSongsAsync(id);
		}

		public async Task<Playlist> CreatePlaylistAsync(string name, string description)
		{
			if (name.Length > 100)
				throw new ArgumentException("Playlist name cannot exceed 100 characters.");
			else if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Playlist name cannot be empty.");
			}
			var playlist = new Playlist(name, description);
			await _playlistRepo.AddAsync(playlist);
			await _playlistRepo.SaveChangesAsync();
			return playlist;
		}
		public async Task UpdatePlaylistAsync(Guid id, string name, string description)
		{
			var playlist = await _playlistRepo.GetByIdAsync(id);
			if (playlist == null) throw new ArgumentException("Playlist not found.");

			playlist.Update(name, description);
			_playlistRepo.Update(playlist);
			await _playlistRepo.SaveChangesAsync();
		}

		public async Task DeletePlaylistAsync(Guid id)
		{
			var playlist = await _playlistRepo.GetByIdAsync(id);
			if (playlist == null) throw new ArgumentException("Playlist not found.");

			_playlistRepo.Delete(playlist);
			await _playlistRepo.SaveChangesAsync();
		}
		// managing songs within playlists
		public async Task AddSongToPlaylistAsync(Guid playlistId, Guid songId)
		{
			var playlist = await _playlistRepo.GetPlaylistWithSongsAsync(playlistId);
			if (playlist == null) throw new ArgumentException("Playlist not found.");

			var song = await _songRepo.GetByIdAsync(songId);
			if (song == null) throw new ArgumentException("Song not found.");

			playlist.AddSong(song);
			await _playlistRepo.SaveChangesAsync();
		}

		public async Task RemoveSongFromPlaylistAsync(Guid playlistId, Guid songId)
		{
			var playlist = await _playlistRepo.GetPlaylistWithSongsAsync(playlistId);
			if (playlist == null) throw new ArgumentException("Playlist not found.");

			var song = playlist.Songs.FirstOrDefault(s => s.Id == songId);
			if (song == null) throw new ArgumentException("Song not found in playlist.");

			playlist.RemoveSong(song);
			await _playlistRepo.SaveChangesAsync();
		}

		// Functions for songs, allowing viewing before add to playlist and creating only
		public async Task<IEnumerable<Song>> GetAllSongsAsync()
		{
			return await _songRepo.GetAllAsync();
		}

		public async Task<Song> CreateSongAsync(string title, string artist, TimeSpan duration)
		{
			var song = new Song(Guid.NewGuid(), title, artist, duration);
			await _songRepo.AddAsync(song);
			await _songRepo.SaveChangesAsync();
			return song;
		}
	}
}
