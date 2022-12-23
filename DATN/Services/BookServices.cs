using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DATN.Services
{
    public class BookServices : IBookServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public BookServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<m_book>> GetAllBook()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_books.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> Create(m_book book)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_books.AddAsync(book);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_book book)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_books.Remove(book);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<int> GetId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                var res = await _context
                .m_books
                .Select(e => e.book_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<bool> Update(m_book book)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_books.Update(book);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<m_book> GetBookById(int id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_books.Where(
                    col => col.book_id.Equals(id)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<IEnumerable<m_book>> GetBookByName(string bookname)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_books.Where(
                    col => col.book_name.Contains(bookname)).ToListAsync();
                return ret;
            }
        }
        public async Task<bool> ExistGenres(int gen_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books.AnyAsync(e => e.genre_id.Equals(gen_id));
            }
        }

        public async Task<bool> ExistSupp(int supp_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books.AnyAsync(e => e.supplier_id.Equals(supp_id));
            }
        }

        public async Task<IEnumerable<m_book>> GetBookByGenre(int genre_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_books.Where(
                    col => col.genre_id.Equals(genre_id)).ToListAsync();
                return ret;
            }
        }

        public async Task<IEnumerable<m_book>> GetBookByListId(List<int> id_list)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books.Where(col => id_list.Contains(col.book_id)).ToListAsync();
            }
        }

        public async Task<bool> Updaterange(List<m_book> book_list)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_books.UpdateRange(book_list);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
        public async Task<IEnumerable<m_book>> GetBookByListName(List<string> bookname)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books.Where(col => bookname.Contains(col.book_name)).ToListAsync();
            }
        }
    }
}
