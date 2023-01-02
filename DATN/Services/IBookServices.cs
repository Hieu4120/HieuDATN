using DATN.Model;

namespace DATN.Services
{
    public interface IBookServices
    {
        Task<bool> Create(m_book book);
        Task<bool> Update(m_book book);
        Task<bool> Delete(m_book book);
        Task<IEnumerable<mediate_book>> GetAllJustBookInf();
        Task<IEnumerable<mediate_book_detail>> GetAllBookWithGenre();
        Task<IEnumerable<m_book>> GetAllBook();
        Task<IEnumerable<mediate_book>> GetBookPriceFilter();
        Task<IEnumerable<m_book>> GetBookByName(string bookname);
        Task<IEnumerable<m_book>> GetBookByListName(List<string> bookname);
        Task<bool> Updaterange(List<m_book> book_list);
        Task<IEnumerable<m_book>> GetBookByGenre(int genre_id);
        Task<m_book> GetBookById(int id);
        Task<IEnumerable<m_book>> GetBookByListId(List<int> id_list);
        Task<mediate_book_detail> bookDetail(int book_id);
        Task<int> GetId();
        Task<bool> ExistGenres(int gen_id);
        Task<bool> ExistSupp(int supp_id);
    }
}
