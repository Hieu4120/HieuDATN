using DATN.Model;

namespace DATN.Services
{
    public interface ICustomerServices
    {
        Task<bool> Create(m_customer customer);
        Task<IEnumerable<m_customer>> GetAllCustomer();
    }
}
