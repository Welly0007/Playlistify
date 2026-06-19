using PlaylistifyAPI.Domain.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;
using PlaylistifyAPI.Domain.Entities;
using System;

namespace Domain.Interfaces.Services
{
	public interface IPlaylistService
	{
		public Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
		public Task<Playlist?> GetPlaylistByIdAsync(Guid id);
		public Task<Playlist> CreatePlaylistAsync(string name, string description);
		public Task AddSongToPlaylistAsync(Guid playlistId, Guid songId);
	}
}
