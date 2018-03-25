using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi
{
    public class MusicContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }

        public MusicContext() : base()
        {
        }

        public MusicContext(DbContextOptions<MusicContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Songs)
                .WithOne(s => s.Artist)
                .IsRequired();
        }
    }
}
