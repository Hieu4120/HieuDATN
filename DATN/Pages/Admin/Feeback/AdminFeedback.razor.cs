using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Feeback
{
    public partial class AdminFeedback
    {
        [Inject]
        private IFeedBackServices ifes { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        private m_feedback feedback_item = new m_feedback();
        Regex regexNumberonly = new Regex("^[0-9]+$");
        private bool isLoading = false;
        private List<string> status = new List<string>()
        {
            "Đã trả lời",
            "Chưa trả lời"
        };
        private int get_feedback_id;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("feedback_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_feedback_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-feeback");
                    return;
                }
            }
            bool CHK_get_feedback_id = await ifes.ExistFeedBack(get_feedback_id);
            if (!CHK_get_feedback_id)
            {
                iredir.RedirectNormal("manager-feeback");
                return;
            }
            feedback_item = await ifes.GetFeedBackById(get_feedback_id);
        }

        private async Task UpdateFeedBack()
        {
            isLoading = true;
            feedback_item.update_at = DateTime.Now;
            await ifes.Update(feedback_item);
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            iredir.RedirectNormal("manager-feeback");
            isLoading = false;
        }
    }
}
