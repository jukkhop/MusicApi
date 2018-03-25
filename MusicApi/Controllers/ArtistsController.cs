using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Produces("application/json")]
    [Route("MusicApi/Artists")]
    public class ArtistsController : Controller
    {
        private readonly MusicContext _context;

        public ArtistsController(MusicContext context)
        {
            _context = context;

            if (_context.Artists.Count() == 0)
            {
                var dummyArtist = new Artist
                {
                    Id = 1,
                    Name = "Auto-generated artist"
                };

                var dummySong = new Song
                {
                    Id = 1,
                    ArtistId = dummyArtist.Id,
                    Name = "Auto-generated song"
                };

                _context.Artists.Add(dummyArtist);
                _context.Songs.Add(dummySong);
                _context.SaveChanges();
            }
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artists = _context.Artists;

            foreach (var artist in artists)
            {
                artist.Songs = await _context.Songs.Where(x => x.ArtistId == artist.Id).ToListAsync<Song>();
            }

            return Ok(artists);
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = await _context.Artists.SingleOrDefaultAsync(m => m.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // POST: api/Artists
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = artist.Id }, artist);
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.Id)
            {
                return BadRequest();
            }

            var entry = _context.Entry(artist);
            entry.State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = await _context.Artists.SingleOrDefaultAsync(m => m.Id == id);
            var songs = await _context.Songs.Where(s => s.ArtistId == id).ToListAsync();

            if (artist == null)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            _context.Songs.RemoveRange(songs);

            await _context.SaveChangesAsync();

            return Ok(artist);
        }

        private bool ArtistExists(long id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}