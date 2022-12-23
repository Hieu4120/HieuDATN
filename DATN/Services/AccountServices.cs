using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DATN;
using DATN.Data;
using DATN.Model;

namespace DATN.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public AccountServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> Create(m_account new_account)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                await _context.m_accounts.AddAsync(new_account);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_account>> GetUserName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var accounts = await _context.m_accounts.Where(mac => mac.username == name).ToListAsync();
                return accounts;
            }

        }
        public async Task<IEnumerable<m_account>> GetAllAcc()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_accounts.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<bool> ExistUser(string userName)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_accounts.AnyAsync(
               usn => usn.username == userName
                );
            }
        }

        public async Task<bool> CheckAcc(string username, string password)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return await _context.m_accounts.AnyAsync(
                ac => ac.username != username || ac.username != password);
            }
        }

        public async Task<m_account> GetCurrentCustomerByName(string name)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                // Idを取得
                var res = await _context
                .m_accounts
                .Where(e => e.username.Equals(name)).FirstOrDefaultAsync();
                return res;
            }
        }

        /* public async Task<int> GetCustomerId()
         {
             using (var _context = _contextFactory.CreateDbContext())
             {
                 // Idを取得
                 var res = await _context
                 .m_books
                 .Select(e => e.book_id).ToListAsync();
                 return res;
             }
         }*/
    }
}

