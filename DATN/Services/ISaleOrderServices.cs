using DATN.Model;

namespace DATN.Services
{
    public interface ISaleOrderServices
    {
        Task<bool> Create(m_sale_order m_sale_Order);
        Task<bool> Update(m_sale_order m_sale_Order);
        Task<bool> Delete(m_sale_order m_sale_Order);
        Task<IEnumerable<m_sale_order>> GetAllsaleOrder();
        Task<int> GetId();
        Task<int> GetTotalSaleOrder();
        Task<List<decimal>> GetTotalPriceSaleOrder();
        Task<m_sale_order> GetById(int id);
    }
}
