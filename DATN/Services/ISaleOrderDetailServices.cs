using DATN.Model;

namespace DATN.Services
{
    public interface ISaleOrderDetailServices
    {
        Task<bool> Create(m_sale_order_detail sale_Order_Detail);
        Task<bool> CreateRange(List<m_sale_order_detail> List_sale_Order_Detail);
        Task<bool> Update(m_sale_order_detail sale_Order_Detail);
        Task<bool> Delete(m_sale_order_detail sale_Order_Detail);
        Task<IEnumerable<m_sale_order_detail>> GetAllDetail();
        Task<IEnumerable<m_sale_order_detail>> GetSaleOrderDetailBySaleOrderId(int salecode);
        Task<int> GetId();
    }
}
