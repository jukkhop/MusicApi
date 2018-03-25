using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Produces("application/json")]
    [Route("MusicApi/Songs")]
    public class SongsController : Controller
    {
        private readonly MusicContext _context;

        public SongsController(MusicContext context)
        {
            _context = context;
        }

        // GET: api/Songs
        // GET: api/Songs?artist=3
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery(Name = "artist")] int artistId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (artistId == 0)
            {
                return Ok(_context.Songs);
            }

            var songs = await _context.Songs.Where(x => x.Id == artistId).ToListAsync();

            return Ok(songs);
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var song = await _context.Songs.SingleOrDefaultAsync(m => m.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        // POST: api/Songs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = song.Id }, song);
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != song.Id)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var song = await _context.Songs.SingleOrDefaultAsync(m => m.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return Ok(song);
        }

        private bool SongExists(long id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}