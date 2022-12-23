using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Book
{
    public partial class AdminMangagerBook
    {
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private IGenreServices ges { get; set; }

        private IEnumerable<m_book>? books;
        private IEnumerable<m_supplier>? suppliers;
        private IEnumerable<m_genre>? genres;
        private bool isLoading = false;
        private int ROW_INDEX = 1;
        private List < string> sup_name = new List<string>();
        private List<string> gen_name = new List<string>();
        private int price_format;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            books = await bs.GetAllBook();
            suppliers = await sups.GetAllSupp();
            genres = await ges.GetAllGenre();
            isLoading = false;
            StateHasChanged();
        }

        private void btn_pass_data_b(m_book ele)
        {
            string book_id = ele.book_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"book_id", book_id},
            };
            iredir.RedirectParameter("update-book", passData);
        }
    }
}
