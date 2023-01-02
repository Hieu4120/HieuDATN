using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Feeback
{
    public partial class AdminManagerFeeback
    {
        [Inject]
        private IFeedBackServices ifes { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private string CurrentUri = "manager-feeback";
        private IEnumerable<m_feedback> feedbacks;
        private IEnumerable<m_feedback> feedbacks_p = new List<m_feedback>();
        private int ROW_INDEX = 1;
        private bool isLoading;

        protected async override Task OnInitializedAsync()
        {
            isLoading = true;
            feedbacks = await ifes.GetAllFeedBack();
            isLoading = false;
            StateHasChanged();
        }

        private void btn_pass_data(m_feedback ele)
        {
            string feed_id = ele.feedback_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"feedback_id", feed_id},
            };
            iredir.RedirectParameter("update-feedback", passData);
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
            pagingInfo.TotalItems = feedbacks.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            feedbacks_p = feedbacks.Skip(skip).Take(PageSize).ToList();
        }
    }
}
