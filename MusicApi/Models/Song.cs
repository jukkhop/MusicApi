
namespace MusicApi.Models
{
    public class Song
    {
        public long Id { get; set; }
        public long ArtistId { get; set; }
        public string Name { get; set; }
        public int LengthSeconds { get; set; }
        public bool IsFavoriteArtist { get; set; }

        public Artist Artist { get; set; }
    }
}
