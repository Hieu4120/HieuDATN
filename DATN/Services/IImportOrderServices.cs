using DATN.Model;

namespace DATN.Services
{
    public interface IImportOrderServices
    {
        Task<bool> Create(m_import_order m_Import_Order);
        Task<bool> Update(m_import_order m_Import_Order);
        Task<bool> Delete(m_import_order m_Import_Order);
        Task<int> GetTotalImportOrder();
        Task<IEnumerable<m_import_order>> GetAllImportOrder();
    }
}
