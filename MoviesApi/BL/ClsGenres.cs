
namespace MoviesApi.BL
{
    public interface IGeners
    {
        public List<Genre> getAll();
        public Genre Save(Genre item);
        public Genre GetById(int id);
        public bool Delete(int id);
    }
    public class ClsGenres : IGeners
    {

        public ApplicationDbContext _context;
        public ClsGenres(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Genre> getAll()
        {
            return _context.Genres.ToList();
        }
        public Genre GetById(int id)
        {
            try
            {
                var item = _context.Genres.FirstOrDefault(i => i.Id == id);
                return item;
            }
            catch
            {
                return new Genre();
            }
        }
        public Genre Save(Genre item)
        {
            try
            {
                if(item.Id == 0)
                {
                    if (item.Name == "")
                        item.Name = "Empty";
                    _context.Genres.Add(item);
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
                _context.Genres.Remove(item);
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
