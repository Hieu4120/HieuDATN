using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Review
{
    public partial class AdminReviewManager
    {
        [Inject]
        private IReviewServices ires { get; set; }
        [Inject]
        private IBookServices ibs { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private string CurrentUri = "manager-review";
        private IEnumerable<mediate_review> reviews;
        private IEnumerable<mediate_review> reviews_p = Enumerable.Empty<mediate_review>();
        private m_book? book_item;
        private List<m_book>? book_list = new List<m_book>();
        private int ROW_INDEX = 1;
        private string status_gen = "";
        private string genId;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            reviews = await ires.GetAllReView();
        }

        /*private void btn_pass_data(m_genre ele)
        {
            string gen_id = ele.genre_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"genre_id", gen_id},
            };
            iredir.RedirectParameter("update-genre", passData);
        }*/

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
            pagingInfo.TotalItems = reviews.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            reviews_p = reviews.Skip(skip).Take(PageSize).ToList();
        }
    }
}
