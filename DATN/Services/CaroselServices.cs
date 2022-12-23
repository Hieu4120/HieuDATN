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

        public async Task<IEnumerable<m_carosel>> GetAllCarousel()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_carosels.ToListAsync();
                    return ret;
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
                var ret = await _context.m_carosels.Where(
                    col => col.carosel_id.Equals(id)).FirstOrDefaultAsync();
                return ret;
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
