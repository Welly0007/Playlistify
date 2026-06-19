using API.DTOs;
using API.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PlaylistController : Controller
	{
		private readonly IPlaylistService _playlistService;
		public PlaylistController(IPlaylistService service)
		{
			_playlistService = service;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllPlaylists()
		{
			var playlists = await _playlistService.GetAllPlaylistsAsync();
			var response = playlists.Select(p => new PlaylistsResultDto
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				TotalDuration = p.TotalDuration,
				SongCount = p.Songs.Count
			}).ToList();
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPlaylist(Guid id)
		{
			var playlist = await _playlistService.GetPlaylistByIdAsync(id);
			if (playlist == null) return NotFound("Playlist not found");

			var response = new PlaylistWithSongsDto
			{
				Id = playlist.Id,
				Name = playlist.Name,
				Description = playlist.Description,
				TotalDuration = playlist.TotalDuration,
				SongCount = playlist.Songs.Count,
				Songs = playlist.Songs.Select(s => new SongResultDto
				{
					Title = s.Title,
					Artist = s.Artist,
					Duration = s.Duration
				}).ToList()
			};

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
		{
			try
			{
				var playlist = await _playlistService.CreatePlaylistAsync(dto.Name, dto.Description);
				var response = new PlaylistsResultDto
				{
					Id = playlist.Id,
					Name = playlist.Name,
					Description = playlist.Description,
					TotalDuration = playlist.TotalDuration,
					SongCount = playlist.Songs.Count
				};
				return Ok(playlist);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("{id}/songs/{songId}")]
		public async Task<IActionResult> AddSongToPlaylist(Guid id, Guid songId)
		{
			try
			{
				await _playlistService.AddSongToPlaylistAsync(id, songId);
				return Ok();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}
