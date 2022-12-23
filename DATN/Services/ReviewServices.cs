using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Services
{
    public class ReviewServices:IReviewServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public ReviewServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(m_review review)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_reviews.AddAsync(review);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_review review)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_reviews.Remove(review);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_review>> GetAllReView()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_reviews.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<int> GetReviewId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                // Idを取得
                var res = await _context
                .m_reviews
                .Select(e => e.review_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<bool> Update(m_review review)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_reviews.Update(review);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
