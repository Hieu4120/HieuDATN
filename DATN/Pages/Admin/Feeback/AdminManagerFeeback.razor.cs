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

        private IEnumerable<m_feedback> feedbacks { get; set; }
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

    }
}
