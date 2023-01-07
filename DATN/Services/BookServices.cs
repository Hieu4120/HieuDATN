using DATN.Data;
using DATN.Model;
using DocumentFormat.OpenXml.Wordprocessing;
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
        public async Task<IEnumerable<mediate_book>> GetAllJustBookInf()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var query = await (
                   from A in _context.m_books
                        .Where(col => col.number_click > 5)
                   select new
                   {
                       book_id = A.book_id,
                       book_name = A.book_name,
                       price = A.price,
                       img_url = A.img_url,
                       number_click = A.number_click
                   }).OrderByDescending(col => col.number_click)
                   .ToListAsync();
                    List<mediate_book> book_list = new List<mediate_book>();
                    foreach (var ele in query)
                    {
                        if (book_list.Count() < 16)
                        {
                            book_list.Add(new mediate_book()
                            {
                                book_id = ele.book_id,
                                book_name = ele.book_name,
                                price = ele.price,
                                img_url = ele.img_url,
                            });
                        }
                    }
                    return book_list;
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
                var ret = await _context.m_books
                    .Where(
                    col => col.book_id.Equals(id))
                    .FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<IEnumerable<m_book>> GetBookByName(string booksearchname)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_books.Where(
                    col => col.book_search_name
                    .Contains(booksearchname) 
                    || col.book_name.Contains(booksearchname))
                    .ToListAsync();
                return ret;
            }
        }
        public async Task<bool> ExistGenres(int gen_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books
                    .AnyAsync(e => e.genre_id
                    .Equals(gen_id));
            }
        }

        public async Task<bool> ExistSupp(int supp_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books
                    .AnyAsync(e => e.supplier_id
                    .Equals(supp_id));
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
                return await _context.m_books
                    .Where(col => id_list
                    .Contains(col.book_id))
                    .ToListAsync();
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
                return await _context.m_books
                    .Where(col => bookname
                    .Contains(col.book_name))
                    .ToListAsync();
            }
        }

        public async Task<mediate_book_detail> bookDetail(int book_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = await (
                    from A in _context.m_books.
                    Where(col => col.book_id == book_id)
                    from B in _context.m_genres
                    .Where(gen => gen.genre_id == A.genre_id)
                    from C in _context.m_suppliers
                    .Where(sup => sup.supplier_id == A.supplier_id)
                    select new
                    {
                        book_name = A.book_name,
                        price = A.price,
                        author = A.author,
                        book_content = A.book_content,
                        img_url = A.img_url,
                        page_number = A.page_number,
                        amount = A.amount,
                        supplier_name = C.supplier_name,
                        genre_name = B.genre_name,
                        release_date = A.release_date,
                        genre_id = B.genre_id,
                    }).FirstOrDefaultAsync();
                mediate_book_detail bookDetail = new mediate_book_detail()
                {
                    book_name = query.book_name,
                    price = query.price,
                    author = query.author,
                    book_content = query.book_content,
                    amount = query.amount,
                    img_url = query.img_url,
                    page_number = query.page_number,
                    supplier_name = query.supplier_name,
                    genre_name = query.genre_name,
                    release_date = query.release_date,
                    genre_id = query.genre_id
                };
                return bookDetail;
            }
        }
        public async Task<IEnumerable<mediate_book_detail>> GetAllBookWithGenre()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = await (
                    from A in _context.m_books
                    from B in _context.m_genres
                    .Where(colg => colg.genre_id == A.genre_id)
                    from C in _context.m_suppliers
                    .Where(cols => cols.supplier_id == A.supplier_id)
                    select new
                    {
                        book_id = A.book_id,
                        book_name = A.book_name,
                        price = A.price,
                        author = A.author,
                        book_content = A.book_content,
                        img_url = A.img_url,
                        page_number = A.page_number,
                        amount = A.amount,
                        supplier_name = C.supplier_name,
                        genre_name = B.genre_name,
                        release_date = A.release_date,
                        genre_id = B.genre_id,
                        status = A.status,
                        number_click = A.number_click
                    }).ToListAsync();
                List<mediate_book_detail> bookDetailList = new List<mediate_book_detail>();
                foreach (var item in query)
                {
                    bookDetailList.Add(new mediate_book_detail()
                    {
                        book_id = item.book_id,
                        book_name = item.book_name,
                        price = item.price,
                        author = item.author,
                        book_content = item.book_content,
                        amount = item.amount,
                        img_url = item.img_url,
                        page_number = item.page_number,
                        supplier_name = item.supplier_name,
                        genre_name = item.genre_name,
                        release_date = item.release_date,
                        genre_id = item.genre_id,
                        status = item.status,
                        number_click = item.number_click
                    });
                }
                return bookDetailList;
            }
        }

        public async Task<IEnumerable<mediate_book>> GetBookPriceFilter()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var query = await (
                   from A in _context.m_books
                   from B in _context.m_genres
                   .Where(colg => colg.genre_id == A.genre_id)
                   select new
                   {
                       book_id = A.book_id,
                       book_name = A.book_name,
                       price = A.price,
                       img_url = A.img_url,
                       number_click = A.number_click,
                       genre_id = B.genre_id
                   }).OrderBy(col => col.number_click)
                   .ToListAsync();
                    List<mediate_book> book_list = new List<mediate_book>();
                    foreach (var ele in query)
                    {
                        book_list.Add(new mediate_book()
                        {
                            book_id = ele.book_id,
                            book_name = ele.book_name,
                            price = ele.price,
                            img_url = ele.img_url,
                            genre_id = ele.genre_id
                        });
                    }
                    return book_list;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
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

        public async Task<bool> ExistBook(int book_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_books
                    .AnyAsync(e => e.book_id
                    .Equals(book_id));
            }
        }
    }
}
