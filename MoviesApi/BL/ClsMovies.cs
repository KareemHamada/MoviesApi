namespace MoviesApi.BL
{
    public interface IMovies { 
        public List<Movie> getAll();
        public Movie Save(Movie item);
        public Movie GetById(int id);
        public bool Delete(int id);
        public List<Movie> GetByGenreId(byte id);
    }
    public class ClsMovies: IMovies
    {
        public ApplicationDbContext _context;
        public ClsMovies(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> getAll()
        {
            return _context.Movies.OrderByDescending(a=>a.Rate).ToList();
        }
        public Movie GetById(int id)
        {
            try
            {
                var item = _context.Movies.FirstOrDefault(i => i.Id == id);
                return item;
            }
            catch
            {
                return new Movie();
            }
        }

        public List<Movie> GetByGenreId(byte id)
        {
            try
            {
                var item = _context.Movies.Where(i => i.GenreId == id).OrderByDescending(a=>a.Rate).ToList();
                return item;
            }
            catch
            {
                return new List<Movie>();
            }
        }
        public Movie Save(Movie item)
        {
            try
            {
                if (item.Id == 0)
                {
                    _context.Movies.Add(item);
                }
                else
                {
                    _context.Entry(item).State = EntityState.Modified;
                }

                _context.SaveChanges();
                return item;
            }
            catch
            {
                return item;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var item = GetById(id);
                _context.Movies.Remove(item);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
