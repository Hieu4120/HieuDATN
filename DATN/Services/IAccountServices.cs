using DATN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Services
{
    public interface IAccountServices
    {
        Task<IEnumerable<m_account>> GetAllAcc();
        Task<bool> Create(m_account new_account);
        Task<IEnumerable<m_account>> GetUserName(string name);
        Task<bool> ExistUser(string userName);
        Task<bool> CheckAcc(string username, string password);
        Task<m_account> GetCurrentCustomerByName( string name);
    }
}