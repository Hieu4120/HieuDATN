using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DATN.Services
{
    public class ImportOrderServices : IImportOrderServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public ImportOrderServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_import_order m_Import_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_import_orders.AddAsync(m_Import_Order);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_import_order m_Import_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_import_orders.Remove(m_Import_Order);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_import_order>> GetAllImportOrder()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_import_orders.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<int> GetTotalImportOrder()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_import_orders.CountAsync();
                return ret;
            }
        }

        public async Task<bool> Update(m_import_order m_Import_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_import_orders.Update(m_Import_Order);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
