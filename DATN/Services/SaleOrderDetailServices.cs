using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Services
{
    public class SaleOrderDetailServices : ISaleOrderDetailServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public SaleOrderDetailServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_sale_order_detail sale_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_sale_order_details.AddAsync(sale_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> CreateRange(List<m_sale_order_detail> List_sale_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_sale_order_details.AddRangeAsync(List_sale_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_sale_order_detail sale_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_sale_order_details.Remove(sale_Order_Detail);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Update(m_sale_order_detail sale_Order_Detail)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_sale_order_details.Update(sale_Order_Detail);
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
                .m_sale_order_details
                .Select(e => e.sale_order_detail_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<IEnumerable<m_sale_order_detail>> GetAllDetail()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_sale_order_details.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<m_sale_order_detail>> GetSaleOrderDetailBySaleOrderId(int salecode)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_sale_order_details.Where(col => col.sale_order_id == salecode).ToListAsync();
                return ret;
            }
        }
    }
}
