using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Context;
using Web_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private ApplicationDbContext? _db;
        public HotelsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/<HotelsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            return await _db.Hotel.ToListAsync(); ;
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> Get(int id)
        {
            Hotel hotel = await _db.Hotel.FirstOrDefaultAsync(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
                return new ObjectResult(hotel);
            }
            _db.Hotel.Add(hotel);
            await _db.SaveChangesAsync();
            return Ok(hotel);
        }

        // POST api/<HotelsController>
        [HttpPost]
        public async Task<ActionResult<Hotel>> Post(Hotel hotel)
        {
            if (hotel == null)
            {
                return BadRequest();
            }
            _db.Hotel.Add(hotel);
            await _db.SaveChangesAsync();
            return Ok(hotel);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> Put(Hotel hotel)
        {
            if (hotel == null)
            {
                return BadRequest();
            }
            if (!_db.Hotel.Any(x => x.Id == hotel.Id))
            {
                return NotFound();
            }
            _db.Hotel.Update(hotel);
            await _db.SaveChangesAsync();
            return Ok(hotel);
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hotel>> Delete (int id)
        {
            Hotel hotel = _db.Hotel.FirstOrDefault(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            _db.Hotel.Remove(hotel);
            await _db.SaveChangesAsync();
            return Ok(hotel);
        }
    }
}
