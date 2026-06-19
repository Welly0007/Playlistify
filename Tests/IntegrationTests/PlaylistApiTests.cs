using System.Net;
using System.Net.Http.Json;
using API.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlaylistifyAPI.Tests.IntegrationTests
{
	public class PlaylistApiTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		public PlaylistApiTests(WebApplicationFactory<Program> factory)
		{
			var testFactory = factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					// swap sql database
					var descriptor = services.SingleOrDefault(
						d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

					if (descriptor != null)
					{
						services.Remove(descriptor);
					}

					// use in-memory database for testing
					services.AddDbContext<AppDbContext>(options =>
					{
						options.UseInMemoryDatabase("PlaylistIntegrationTestingDb");
					});
				});
			});

			_client = testFactory.CreateClient();
		}

		[Fact]
		public async Task CreatePlaylist_WithValidData_ReturnsOkAndPayload()
		{
			var payload = new CreatePlaylistDto
			{
				Name = "Synthwave Retro",
				Description = "Late night driving music"
			};

			var response = await _client.PostAsJsonAsync("/api/playlist", payload);

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			// verifies it returns the correct payload with a valid ID and song count of 0
			var content = await response.Content.ReadFromJsonAsync<PlaylistsResultDto>();
			Assert.NotNull(content);
			Assert.Equal("Synthwave Retro", content.Name);
			Assert.NotEqual(Guid.Empty, content.Id);
			Assert.Equal(0, content.SongCount);
		}

		[Fact]
		public async Task GetPlaylist_WhenDoesNotExist_ReturnsNotFound()
		{
			var missingId = Guid.NewGuid();

			var response = await _client.GetAsync($"/api/playlist/{missingId}");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task AddSongToPlaylist_InvalidIds_ReturnsBadRequest()
		{
			var fakePlaylistId = Guid.NewGuid();
			var fakeSongId = Guid.NewGuid();


			var response = await _client.PostAsJsonAsync($"/api/playlist/{fakePlaylistId}/songs/{fakeSongId}", "");

			// check if it returns bad request since the playlist and song do not exist
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task UpdatePlaylist_WhenTargetIsMissing_ReturnsBadRequest()
		{
			var missingId = Guid.NewGuid();
			var payload = new CreatePlaylistDto { Name = "Updated Name", Description = "Updated Desc" };

			var response = await _client.PutAsJsonAsync($"/api/playlist/{missingId}", payload);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}