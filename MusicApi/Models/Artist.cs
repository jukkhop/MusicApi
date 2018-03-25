using System.Collections.Generic;

namespace MusicApi.Models
{
    public class Artist
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
