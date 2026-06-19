using Microsoft.EntityFrameworkCore;
using PlaylistifyAPI.Domain.Entities;

namespace Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<Playlist> Playlists { get; set; }
		public DbSet<Song> Songs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Playlist>()
				.HasMany(p => p.Songs)
				.WithMany(s => s.Playlists)
				.UsingEntity<Dictionary<string, object>>(
				"PlaylistSong",
				j => j.HasOne<Song>().WithMany().HasForeignKey("SongId"),
				j => j.HasOne<Playlist>().WithMany().HasForeignKey("PlaylistId")
				);
		}

	}
}
