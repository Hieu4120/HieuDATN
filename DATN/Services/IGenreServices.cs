using DATN.Model;

namespace DATN.Services
{
    public interface IGenreServices
    {
        Task<bool> Create(m_genre genre);
        Task<bool> Update(m_genre genre);
        Task<bool> Delete(m_genre genre);
        Task<int> GetGenId();
        Task<m_genre> GetById(int g_id);
        Task<IEnumerable<m_genre>> GetAllGenre();
        Task<IEnumerable<mediate_genre>> GetAllGenreName();
        Task<bool> ExistName(string g_name);
    }
}
