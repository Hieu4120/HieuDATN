using DATN.Model;

namespace DATN.Services
{
    public interface ICarouselSevices
    {
        Task<bool> Create(m_carosel carosel);
        Task<bool> Update(m_carosel carosel);
        Task<bool> Delete(m_carosel carosel);
        Task<IEnumerable<m_carosel>> GetAllCarousel();
        Task<m_carosel> GetCarouselById(int id);
    }
}
