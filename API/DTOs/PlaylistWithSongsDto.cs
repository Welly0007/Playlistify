using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class PlaylistWithSongsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SongCount { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public IEnumerable<SongResultDto> Songs { get; set; } = new List<SongResultDto>();
    }
}
