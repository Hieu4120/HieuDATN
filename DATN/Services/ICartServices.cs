using DATN.Model;

namespace DATN.Services
{
    public interface ICartServices
    {
        Task<bool> Create(m_cart cart);
        Task<bool> Update(m_cart cart);
        Task<bool> Delete(m_cart cart);
        Task<bool> DeleteRange(List<m_cart>  cart_list);
        Task<int> GetCartId();
        Task<bool> ExistCartItemCHK(int id);
        Task<IEnumerable<m_cart>> GetAllCart();
        Task<m_cart> GetCartItembyBookId(int bookid);
        Task<IEnumerable<m_cart>> GetCartItembyCusId(int cus_id);
        Task<IEnumerable<m_mediate_checkout>> GetCartItem(int cus_id);
    }
}
