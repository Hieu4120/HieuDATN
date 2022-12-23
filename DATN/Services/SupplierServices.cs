using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Services
{
    public class SupplierServices : ISupplierServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public SupplierServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(m_supplier supp)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_suppliers.AddAsync(supp);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_supplier supp)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_suppliers.Remove(supp);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> ExistName(string s_name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_suppliers.AnyAsync(e => e.supplier_name.Equals(s_name));
            }
        }

        public async Task<IEnumerable<m_supplier>> GetAllSupp()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_suppliers.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<int> GetSupId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = await _context
                .m_suppliers
                .Select(e => e.supplier_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<m_supplier> GetSuppById(int s_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_suppliers.Where(
                    col => col.supplier_id.Equals(s_id)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<bool> Update(m_supplier supp)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_suppliers.Update(supp);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
