using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PlaylistifyAPI.Domain.Entities;

namespace Infrastructure.Repositories
{
	public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
	{
		public PlaylistRepository(AppDbContext context) : base(context)
		{ }
		public async Task<Playlist?> GetPlaylistWithSongsAsync(Guid id)
		{
			return await _dbSet.Include(p => p.Songs).FirstOrDefaultAsync(p => p.Id == id);
		}
	}
}
