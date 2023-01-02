using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DATN.Services
{
    public class GenreServices : IGenreServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public GenreServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(m_genre genre)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_genres.AddAsync(genre);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_genre genre)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_genres.Remove(genre);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> ExistName(string g_name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_genres.AnyAsync(e => e.genre_name.Equals(g_name));
            }
        }

        public async Task<IEnumerable<m_genre>> GetAllGenre()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_genres.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<mediate_genre>> GetAllGenreName()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var query = await(
                   from B in _context.m_genres
                   select new
                   {
                       gen_name  = B.genre_name,
                       gen_id = B.genre_id
                   }).ToListAsync();
                    List<mediate_genre> genre_list = new List<mediate_genre>();
                    foreach (var ele in query)
                    {
                        genre_list.Add(new mediate_genre()
                        {
                            genre_name = ele.gen_name,
                            genre_id = ele.gen_id
                        });
                    }
                    return genre_list;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<m_genre> GetById(int g_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_genres.Where(
                    col => col.genre_id.Equals(g_id)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<int> GetGenId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = await _context
                .m_genres
                .Select(e => e.genre_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }

        }

        public async Task<bool> Update(m_genre genre)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_genres.Update(genre);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
