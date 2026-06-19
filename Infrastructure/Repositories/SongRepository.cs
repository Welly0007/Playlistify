using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using PlaylistifyAPI.Domain.Entities;

namespace Infrastructure.Repositories
{
	public class SongRepository : GenericRepository<Song>, ISongRepository
	{
		public SongRepository(AppDbContext context) : base(context)
		{ }
	}
}
