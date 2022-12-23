using BlazorStrap;
using DATN.Model;
using DATN.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class Contact
    {
        [Inject]
        private IFeedBackServices? ifes { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IAccountServices iacs { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private m_feedback m_Feedback = new m_feedback();
        private m_account? m_Account = new m_account();
        private string user;
        private int feedback_id_init;
        private async void btn_add_feedback()
        {
            var authState = await authenticationStateTask;

            user = authState.User.Identity.Name;
            if (user == null)
            {
                ino.Notify((NotificationSeverity.Success, "Bạn vẫn chưa đăng nhập"));
                return;
            }
            else
            {
                if (feedback_id_init != null)
                {
                    feedback_id_init = await ifes.GetFeedBackId();
                }
                else
                {
                    feedback_id_init = 1;
                }
                m_Account = await iacs.GetCurrentCustomerByName(user);
                m_Feedback.feedback_id = feedback_id_init + 1;
                m_Feedback.customer_id = m_Account.customer_id;
                m_Feedback.status = "Chưa trả lời";
                m_Feedback.update_at = DateTime.Now;
                await ifes.Create(m_Feedback);
                ino.Notify((NotificationSeverity.Success, "Gửi thành công"));
                m_Feedback = new m_feedback();
                StateHasChanged();
            }
        }
    }
}
