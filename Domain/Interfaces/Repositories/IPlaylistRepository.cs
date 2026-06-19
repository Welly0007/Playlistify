using PlaylistifyAPI.Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IPlaylistRepository : IGenericRepository<Playlist>
	{
		Task<Playlist?> GetPlaylistWithSongsAsync(Guid id);
	}
}
