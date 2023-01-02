using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Book
{
    public partial class AdminMangagerBook
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IBookServices bs { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private IEnumerable<mediate_book_detail> detailbooklis_i = Enumerable.Empty<mediate_book_detail>();
        private IEnumerable<mediate_book_detail>? books;
        private bool isLoading = false;
        private int ROW_INDEX = 1;
        private string CurrentUri = "manager-book";
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            books = await bs.GetAllBookWithGenre();
            isLoading = false;
            StateHasChanged();
        }

        private void btn_pass_data_b(int book_id)
        {
            string book_id_pass = book_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"book_id", book_id_pass},
            };
            iredir.RedirectParameter("update-book", passData);
        }

        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }
        public async void CreatePagingInfo()
        {
            int PageSize = 4;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = books.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            detailbooklis_i = books.Skip(skip).Take(PageSize).ToList();
        }
    }
}
