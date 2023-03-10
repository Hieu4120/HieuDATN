using DATN.Model;

namespace DATN.Services
{
    public interface IReviewServices
    {
        Task<bool> Create(m_review review);
        Task<bool> Delete(m_review review);
        Task<int> GetReviewId();
        Task<IEnumerable<mediate_review>> GetAllReView();
        Task<IEnumerable<m_review>> GetReViewByBookId(int bookId);
    }
}
