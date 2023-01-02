using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace DATN.Services
{
    public class ImportOrderDetailServices : IImportOrderDetailServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public ImportOrderDetailServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(m_import_order_detail import_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_import_order_details.AddAsync(import_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_import_order_detail import_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_import_order_details.Remove(import_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_import_order_detail>> GetImportDetailByImportOrderId(int importcode)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_import_order_details.Where(col => col.import_order_id == importcode).ToListAsync();
                return ret;
            }
        }

        public async Task<int> GetId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                var res = await _context
                .m_import_order_details
                .Select(e => e.import_order_detail_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<bool> Update(m_import_order_detail import_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_import_order_details.Update(import_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> CheckBookNameExist(string book_name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_import_order_details.AnyAsync(e => e.book_name.Equals(book_name));
            }
        }
        public async Task<bool> CheckBookIdExist(int book_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_import_order_details.AnyAsync(e => e.book_id.Equals(book_id));
            }
        }
        public async Task<bool> CheckImportOrderId(int import_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_import_order_details.AnyAsync(e => e.import_order_id.Equals(import_id));
            }
        }
        public async Task<IEnumerable<m_import_order_detail>> search(string key)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ele = await _context.m_import_order_details
                .Where(item => item.book_name.Contains(key))
                .ToListAsync();

                return ele;
            }
        }

        public async Task<m_import_order_detail> GetImportDetailByBookId(int bookid)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_import_order_details.Where(col => col.book_id == bookid).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<bool> CreateRange(List<m_import_order_detail> List_import_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_import_order_details.AddRangeAsync(List_import_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> UpdateRange(List<m_import_order_detail> List_import_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_import_order_details.UpdateRange(List_import_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_import_order_detail>> GetImportDetailByListIdBook(List<int> book_list_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_import_order_details.Where(col => book_list_id.Contains((int)col.book_id)).ToListAsync();
            }
        }

        public async Task<IEnumerable<mediate_import_order_detail>> GetImportDetailByImportOrderId1(int importcode)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = await (
                    from A in _context.m_import_order_details.
                    Where(col => col.import_order_id == importcode)
                    from B in _context.m_genres
                    .Where(gen => gen.genre_id == A.genre_id)
                    from C in _context.m_suppliers
                    .Where(sup => sup.supplier_id == A.supplier_id)
                    select new
                    {
                        import_order_id = A.import_order_id,
                        book_id = A.book_id,
                        book_name = A.book_name,
                        price = A.price,
                        author = A.author,
                        book_image = A.book_image,
                        page_number = A.page_number,
                        amount = A.amount,
                        supplier_name = C.supplier_name,
                        genre_name = B.genre_name,
                    }).ToListAsync();
                List<mediate_import_order_detail> List_import_detail = new List<mediate_import_order_detail>();

                foreach (var item in query)
                {
                    List_import_detail.Add( new mediate_import_order_detail()
                    {
                        import_order_id = item.import_order_id,
                        book_id = item.book_id,
                        book_name = item.book_name,
                        price = item.price,
                        author = item.author,
                        amount = item.amount,
                        book_image = item.book_image,
                        page_number = item.page_number,
                        supplier_name = item.supplier_name,
                        genre_name = item.genre_name,
                    });
                }
                return List_import_detail;
            }
        }
    }
}
