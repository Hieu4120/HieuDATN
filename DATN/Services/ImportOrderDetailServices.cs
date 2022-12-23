using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace DATN.Services
{
    public class ImportOrderDetailServices: IImportOrderDetailServices
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
    }
}
