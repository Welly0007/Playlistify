using Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
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

		public async Task AddSongToPlaylistAsync(Guid playlistId, Guid songId)
		{
			var playlist = await _playlistRepo.GetPlaylistWithSongsAsync(playlistId);
			if (playlist == null) throw new ArgumentException("Playlist not found.");

			var song = await _songRepo.GetByIdAsync(songId);
			if (song == null) throw new ArgumentException("Song not found.");

			playlist.AddSong(song);
			await _playlistRepo.SaveChangesAsync();
		}
	}
}
