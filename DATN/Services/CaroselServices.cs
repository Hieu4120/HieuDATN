using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Services
{
    public class CaroselServices : ICarouselSevices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public CaroselServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_carosel carosel)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                await _context.m_carosels.AddAsync(carosel);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_carosel carosel)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_carosels.Remove(carosel);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> ExistCarousel(int carou_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_carosels
                    .AnyAsync(e => e.carosel_id
                    .Equals(carou_id));
            }
        }

        public async Task<IEnumerable<m_carosel>> GetAllCarousel()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var query = await (
                   from A in _context.m_carosels
                   select new
                   {
                       carosel_id = A.carosel_id,
                       tiltle = A.tiltle,
                       content = A.content,
                       caroimg_url = A.caroimg_url,
                   }).ToListAsync();
                    List<m_carosel> Caro_list = new List<m_carosel>();
                    foreach (var ele in query)
                    {
                        if (Caro_list.Count() < 16)
                        {
                            Caro_list.Add(new m_carosel()
                            {
                                carosel_id = ele.carosel_id,
                                tiltle = ele.tiltle,
                                content = ele.content,
                                caroimg_url = ele.caroimg_url,
                            });
                        }
                    }
                    return Caro_list;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<m_carosel> GetCarouselById(int id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = await (
                  from A in _context.m_carosels
                  .Where(col => col.carosel_id == id)
                  select new
                  {
                      tiltle = A.tiltle,
                      content = A.content,
                      caroimg_url = A.caroimg_url,
                  }).FirstOrDefaultAsync();
                m_carosel Caro_item = new m_carosel()
                {
                    tiltle = query.tiltle,
                    content = query.content,
                    caroimg_url = query.caroimg_url,

                };
                return Caro_item;
            }
        }

        public async Task<bool> Update(m_carosel carosel)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_carosels.Update(carosel);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
