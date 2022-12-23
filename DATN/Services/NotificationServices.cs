using Microsoft.VisualBasic;
using Radzen;

namespace DATN.Services
{
    public class NotificationServices
    {
        public interface INotificationService
        {
            void NotifyError(string detail);
            void NotifyInfo(string detail);
            void NotifySuccess(string detail);
            void NotifyWarning(string detail);
            void Notify((NotificationSeverity Severity, string Message) notification);
        }

        public class NotiService : INotificationService
        {
            private readonly Radzen.NotificationService _notificationService;

            public NotiService(
                Radzen.NotificationService notificationService
            )
            {
                _notificationService = notificationService;
            }

            private void ShowNotification(NotificationSeverity severity, string summary, string detail)
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = severity,
                    Summary = summary,
                    Detail = detail,
                    Duration = 3000
                });
            }

            public void NotifyError(string detail)
            {
                ShowNotification(NotificationSeverity.Error, "Lỗi", detail);
            }

            public void NotifyInfo(string detail)
            {
                ShowNotification(NotificationSeverity.Info, "Thông báo", detail);
            }

            public void NotifySuccess(string detail)
            {
                ShowNotification(NotificationSeverity.Success, "Thông báo", detail);
            }

            public void NotifyWarning(string detail)
            {
                ShowNotification(NotificationSeverity.Warning, "Cảnh báo", detail);
            }

            public void Notify((NotificationSeverity Severity, string Message) notification)
            {
                switch (notification.Severity)
                {
                    case NotificationSeverity.Error:
                        NotifyError(notification.Message);
                        break;

                    case NotificationSeverity.Info:
                        NotifyInfo(notification.Message);
                        break;

                    case NotificationSeverity.Success:
                        NotifySuccess(notification.Message);
                        break;

                    case NotificationSeverity.Warning:
                        NotifyWarning(notification.Message);
                        break;
                }
            }
        }
    }
}
