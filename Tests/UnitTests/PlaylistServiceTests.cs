using Domain.Interfaces.Repositories;
using Infrastructure.Services;
using Moq;
using PlaylistifyAPI.Domain.Entities;

namespace PlaylistifyAPI.Tests.UnitTests
{
	public class PlaylistServiceTests
	{
		private readonly Mock<IPlaylistRepository> _playlistRepoMock;
		private readonly Mock<ISongRepository> _songRepoMock;
		private readonly PlaylistService _service;

		public PlaylistServiceTests()
		{
			_playlistRepoMock = new Mock<IPlaylistRepository>();
			_songRepoMock = new Mock<ISongRepository>();

			_service = new PlaylistService(_playlistRepoMock.Object, _songRepoMock.Object);
		}

		[Fact]
		public async Task CreatePlaylist_WithValidData_ReturnsPlaylist()
		{
			var result = await _service.CreatePlaylistAsync("My Rock Mix", "Some cool rock songs");

			Assert.NotNull(result);
			Assert.Equal("My Rock Mix", result.Name);

			_playlistRepoMock.Verify(r => r.AddAsync(It.IsAny<Playlist>()), Times.Once);
			_playlistRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
		}

		[Fact]
		public async Task AddSongToPlaylist_PlaylistNotFound_ThrowsArgumentException()
		{
			var playlistId = Guid.NewGuid();
			var songId = Guid.NewGuid();

			_playlistRepoMock.Setup(r => r.GetPlaylistWithSongsAsync(playlistId))
							 .ReturnsAsync((Playlist)null);

			var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
				_service.AddSongToPlaylistAsync(playlistId, songId));

			Assert.Equal("Playlist not found.", ex.Message);
		}

		[Fact]
		public async Task AddSongToPlaylist_WithValidIds_AddsSongAndSaves()
		{
			var playlist = new Playlist("Workout Tunes", "Gym music");
			var song = new Song(Guid.NewGuid(), "Sandstorm", "Darude", TimeSpan.FromMinutes(3));

			_playlistRepoMock.Setup(r => r.GetPlaylistWithSongsAsync(playlist.Id)).ReturnsAsync(playlist);
			_songRepoMock.Setup(r => r.GetByIdAsync(song.Id)).ReturnsAsync(song);

			await _service.AddSongToPlaylistAsync(playlist.Id, song.Id);

			Assert.Contains(song, playlist.Songs);
			_playlistRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
		}
	}
}