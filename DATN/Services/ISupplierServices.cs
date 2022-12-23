using DATN.Model;

namespace DATN.Services
{
    public interface ISupplierServices
    {
        Task<bool> Create( m_supplier supp);
        Task<bool> Update( m_supplier supp);
        Task<bool> Delete( m_supplier supp);
        Task<int> GetSupId();
        Task< m_supplier> GetSuppById(int s_id);
        Task<IEnumerable< m_supplier>> GetAllSupp();
        Task<bool> ExistName(string s_name);
    }
}
