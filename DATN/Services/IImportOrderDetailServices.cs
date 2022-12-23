using DATN.Model;

namespace DATN.Services
{
    public interface IImportOrderDetailServices
    {
        Task<bool> Create(m_import_order_detail import_Order_Detail);
        Task<bool> CreateRange(List<m_import_order_detail> List_import_Order_Detail);
        Task<bool> UpdateRange(List<m_import_order_detail> List_import_Order_Detail);
        Task<bool> Update(m_import_order_detail import_Order_Detail);
        Task<bool> Delete(m_import_order_detail import_Order_Detail);
        Task<IEnumerable<m_import_order_detail>> GetImportDetailByImportOrderId(int importcode);
        Task<IEnumerable<m_import_order_detail>> GetImportDetailByListIdBook(List<int> book_list_id);
        Task<m_import_order_detail> GetImportDetailByBookId(int bookid);
        Task<bool> CheckBookNameExist(string book_name);
        Task<bool> CheckBookIdExist(int book_id);
        Task<bool> CheckImportOrderId(int import_id);
        Task<IEnumerable<m_import_order_detail>> search(string key);
        Task<int> GetId();
    }
}
