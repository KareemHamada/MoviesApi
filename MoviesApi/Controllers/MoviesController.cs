
namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private List<string> _allowedExtension = new List<string>() { ".jpg",".png"};
        private long _maxAllowdPosterSize = 1048576;
        IMovies _oMovies;
        IGeners _oGeners;
        public MoviesController(IMovies oMovies, IGeners oGeners)
        {
            _oMovies = oMovies;
            _oGeners= oGeners;
        }


        [HttpPost]
        public async Task<IActionResult> post([FromForm] MovieModel mov)
        {
            
            var isValidGenre = _oGeners.GetById(mov.GenreId);
            if (isValidGenre == null)
            {
                return BadRequest("Invalid Genre ID");
            }
            if (mov.Poster == null)
                return BadRequest("Poster is required");
            if (!_allowedExtension.Contains(Path.GetExtension(mov.Poster.FileName).ToLower()))
            {
                return BadRequest("only .png and .jpg images are allowed");
            }
            if(mov.Poster.Length > _maxAllowdPosterSize) {
                return BadRequest("Max allowed size for poster is 1MB");
            }
            using var dataStream = new MemoryStream();
            await mov.Poster.CopyToAsync(dataStream);


            var movie = new Movie {
                GenreId = mov.GenreId,
                Title = mov.Title,
                Poster = dataStream.ToArray(),
                Rate = mov.Rate,
                StoreLine = mov.StoreLine,
                Year = mov.Year
            };

            var m = _oMovies.Save(movie);

            return Ok(m);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _oMovies.getAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _oMovies.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("{GetByGenre}")]
        public IActionResult GetByGenreId(byte id)
        {
            var item = _oMovies.GetByGenreId(id);
            if (item.Count <= 0)
                return NotFound();

            return Ok(item);
        }


        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var findItem = _oMovies.GetById(id);
            if (findItem == null)
                return NotFound($"No movie was found with Id : {id}");

            _oMovies.Delete(id);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> put(int id, [FromForm] MovieModel mov)
        {
            var movie = _oMovies.GetById(id);
            if (movie == null)
                return NotFound($"No movie was found with Id : {id}");
            var isValidGenre = _oGeners.GetById(mov.GenreId);
            if (isValidGenre == null)
            {
                return BadRequest("Invalid Genre ID");
            }
            if(mov.Poster != null)
            {
                if (!_allowedExtension.Contains(Path.GetExtension(mov.Poster.FileName).ToLower()))
                {
                    return BadRequest("only .png and .jpg images are allowed");
                }
                if (mov.Poster.Length > _maxAllowdPosterSize)
                {
                    return BadRequest("Max allowed size for poster is 1MB");
                }

                using var dataStream = new MemoryStream();
                await mov.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
            movie.Title = mov.Title;
            movie.Rate = mov.Rate;
            movie.GenreId = mov.GenreId;
            movie.Year = mov.Year;
            movie.StoreLine = mov.StoreLine;


            var gen = _oMovies.Save(movie);

            return Ok(gen);
        }
    }
}
