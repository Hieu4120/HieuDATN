using BlazorStrap;
using DATN.Data;
using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATN.Services
{
    public class FeedBackServices : IFeedBackServices
    {
        private readonly IDbContextFactory<BookDBContext> _contextFactory;
        public FeedBackServices(IDbContextFactory<BookDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> Create(m_feedback feedback)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {

                bool ret = false;
                await _context.m_feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<bool> Delete(m_feedback feedback)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                var del = _context.m_feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }

        public async Task<IEnumerable<m_feedback>> GetAllFeedBack()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var ret = await _context.m_feedbacks.ToListAsync();
                    return ret;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public async Task<m_feedback> GetFeedBackById(int feedback_id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var ret = await _context.m_feedbacks.Where(
                    col => col.feedback_id.Equals(feedback_id)).FirstOrDefaultAsync();
                return ret;
            }
        }

        public async Task<int> GetFeedBackId()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = await _context
                .m_feedbacks
                .Select(e => e.feedback_id).ToListAsync();
                int max = 0;
                if (res != null && res.Count != 0)
                {
                    max = (int)res.Max();
                }
                return max;
            }
        }

        public async Task<bool> Update(m_feedback feedback)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                bool ret = false;
                _context.m_feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
                ret = true;
                return ret;
            }
        }
    }
}
