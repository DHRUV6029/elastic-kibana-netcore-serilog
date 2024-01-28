using Elastic.Kibana.Serilog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elastic.Kibana.Serilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _dbContext;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieContext dbContext  , ILogger<MovieController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            _logger.LogInformation("Api GetMovies called");
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Api GetMovies end");
            return await _dbContext.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            _logger.LogInformation("Api GetMovies with id {id} called", id);
            if (_dbContext.Movies == null)
            {
                _logger.LogInformation("No Movie in the Table");
                return NotFound();
            }
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                _logger.LogInformation($"Movie {id}" + "not Found");
                return NotFound();
            }
            _logger.LogInformation("Movie, {id} " + " {name}", id , movie.Title );
            _logger.LogInformation("Api GetMovies with id: {id} end", id);
            return movie;
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Saved Movie with id : {id}", movie.Id);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            _logger.LogInformation("Updating Content of Movie with id: {id}", id);
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("Updated Content of Movie with id: {id}", id);
            return NoContent();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Deleted Movie with id {id}", id);
            return NoContent();
        }

        private bool MovieExists(long id)
        {
            _logger.LogInformation("Checking if Movie is Present with id {id}", id);
            return (_dbContext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
