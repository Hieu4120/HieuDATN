using DATN.Data;
using DATN.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;

namespace DATN.Services
{
    public class SaleOrderServices : ISaleOrderServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public SaleOrderServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_sale_order m_sale_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                await _context.m_sale_orders.AddAsync(m_sale_Order);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_sale_order m_sale_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_sale_orders.Remove(m_sale_Order);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_sale_order>> GetAllsaleOrder()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_sale_orders.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> Update(m_sale_order m_sale_Order)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_sale_orders.Update(m_sale_Order);
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
                .m_sale_orders
                .Select(e => e.sale_order_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<m_sale_order> GetById(int id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_sale_orders.Where(
                    col => col.sale_order_id.Equals(id)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<int> GetTotalSaleOrder()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_sale_orders.CountAsync();
                return ret;
            }
        }

        public async Task<List<decimal>> GetTotalPriceSaleOrder()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_sale_orders.Select(col => col.price).ToListAsync();
                return ret;
            }
        }
    }
}
