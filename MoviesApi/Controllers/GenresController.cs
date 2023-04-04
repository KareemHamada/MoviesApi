

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        IGeners _oGeners;

        public GenresController(IGeners oGeners)
        {
            _oGeners = oGeners;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _oGeners.getAll();
            return Ok(items);
        }
        [HttpPost]
        public IActionResult post([FromBody] Genre genre)
        {

            var gen = _oGeners.Save(genre);

            return Ok(gen);
        }


        [HttpPut("{id}")]
        public IActionResult put(byte id, [FromBody] Genre genre)
        {
            var findGenre = _oGeners.GetById(id);
            if (findGenre == null)
                return NotFound($"No genre was found with Id : {id}");

            findGenre.Name = genre.Name;
            var gen = _oGeners.Save(findGenre);

            return Ok(gen);
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var findGenre = _oGeners.GetById(id);
            if (findGenre == null)
                return NotFound($"No genre was found with Id : {id}");

            _oGeners.Delete(id);
            return Ok();
        }


    }
}
