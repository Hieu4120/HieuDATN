using DATN.Data;
using DATN.Model;
using DATN.Pages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DATN.Services
{
    public class CartServices : ICartServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public CartServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_cart cart)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                await _context.m_carts.AddAsync(cart);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_cart cart)
        { 
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_carts.Remove(cart);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<int> GetCartId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = await _context
               .m_carts
               .Select(e => e.cart_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<bool> Update(m_cart cart)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_carts.Update(cart);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
        public async Task<bool> ExistCartItemCHK(int id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_carts.AnyAsync(e => e.book_id.Equals(id));
            }
        }

        public async Task<IEnumerable<m_cart>> GetAllCart()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_carts.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<m_cart> GetCartItembyBookId(int bookid)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_carts.Where(
                    col => col.book_id.Equals(bookid)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<IEnumerable<m_cart>> GetCartItembyCusId(int cus_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_carts.Where(
                    col => col.customer_id.Equals(cus_id)).ToListAsync();
                return ret;
            }
        }

        public async Task<IEnumerable<m_mediate_checkout>> GetCartItem(int cus_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = (
                    from M in _context.m_carts
                    .Where(ca => ca.customer_id.Equals(cus_id))
                    from N in _context.m_books
                    .Where(col => col.book_id == M.book_id)
                    select new
                    {
                        M.amount,
                        N.book_name,
                        N.price
                    })
                    .ToList();
                List<m_mediate_checkout> ListCheckout = new List<m_mediate_checkout>();
                foreach(var ele in query)
                {
                    m_mediate_checkout checkout = new m_mediate_checkout
                    {
                        book_name = ele.book_name,
                        amount = ele.amount,
                        price = ele.price
                    };
                    ListCheckout.Add(checkout);
                }
                return ListCheckout;
            }
        }

        public async Task<bool> DeleteRange(List<m_cart> cart_list)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_carts.RemoveRange(cart_list);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
