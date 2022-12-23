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

        private IEnumerable<m_review> reviews;
        private m_book? book_item;
        private List<m_book>? book_list = new List<m_book>();
        private int ROW_INDEX = 1;
        private string status_gen = "";
        private string genId;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            reviews = await ires.GetAllReView();
            foreach( var review in reviews)
            {
                book_item = await ibs.GetBookById(review.book_id);
                book_list.Add(book_item);
            }
        }

        private void btn_pass_data(m_genre ele)
        {
            string gen_id = ele.genre_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"genre_id", gen_id},
            };
            iredir.RedirectParameter("update-genre", passData);
        }
    }
}
