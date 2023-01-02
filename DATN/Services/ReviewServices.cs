using DATN.Data;
using DATN.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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

        public async Task<IEnumerable<mediate_review>> GetAllReView()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var query = await (
                   from A in _context.m_reviews
                   from B in _context.m_books
                   .Where(bs => bs.book_id == A.book_id)
                   select new
                   {
                       book_name = B.book_name,
                       review_content = A.review_content,
                       user_name = A.user_name,
                       email = A.email,
                       rating = A.rating
                   }).ToListAsync();
                    List<mediate_review> reviewList = new List<mediate_review>();
                    foreach(var ele in query)
                    {
                        reviewList.Add(new mediate_review()
                        {
                            book_name = ele.book_name,
                            review_content = ele.review_content,
                            user_name = ele.user_name,
                            email = ele.email,
                            rating = ele.rating
                        });
                    }
                    return reviewList;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<m_review>> GetReViewByBookId(int bookId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_reviews.Where(
                    col => col.book_id.Equals(bookId)).ToListAsync();
                return ret;
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
