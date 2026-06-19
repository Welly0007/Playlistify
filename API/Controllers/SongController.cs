using API.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SongController : Controller
	{
		private readonly IPlaylistService _playlistService;
		public SongController(IPlaylistService service)
		{
			_playlistService = service;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllSongs()
		{
			var songs = await _playlistService.GetAllSongsAsync();
			var response = songs.Select(s => new SongResultDto
			{
				Id = s.Id,
				Title = s.Title,
				Artist = s.Artist,
				Duration = s.Duration
			}).ToList();

			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSong([FromBody] CreateSongDto dto)
		{
			try
			{
				var song = await _playlistService.CreateSongAsync(dto.Title, dto.Artist, dto.Duration);
				return Ok(new { song.Id, song.Title, song.Artist, song.Duration });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}