using DATN.Model;

namespace DATN.Services
{
    public interface IFeedBackServices
    {
        Task<bool> Create(m_feedback feedback);
        Task<bool> Update(m_feedback feedback);
        Task<bool> Delete(m_feedback feedback);
        Task<int> GetFeedBackId();
        Task<m_feedback> GetFeedBackById(int feedback_id);
        Task<IEnumerable<m_feedback>> GetAllFeedBack();
    }
}
