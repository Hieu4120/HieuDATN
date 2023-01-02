using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Exchange;
using Microsoft.Exchange.WebServices.Data;

namespace DATN.Services
{
    public class EXChangeMailService
    {
        private IConfiguration _conf { get; set; }
        private bool _isDevelop;
        private string urlBase;
        public EXChangeMailService()
        {
            //_conf = conf;
        }
        public async Task<bool> SendMail(List<string> emailAccounts)
        {
            string MailUser = "tronghieu2019222@outlook.com";
            string MailPass = "Hieu4120@";
            var subject = "Cảm ơn bạn đã đặt hàng tại HieuShop";
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                service.Credentials = new WebCredentials(MailUser, MailPass);
                service.AutodiscoverUrl(MailUser, RedirectionUrlValidationCallback);
                EmailMessage emailMessage = new EmailMessage(service);
                emailMessage.Subject = subject;

                emailMessage.Body = new MessageBody("Đơn hàng của bạn đang được xử lý <br /> " +
                    "Vui lòng chờ cho đến khi đơn hàng được giao <br/> " +
                    "Nếu sau 2 ngày bạn vẫn không nhận được hàng,<br/> " +
                    "xin vui lòng liên hệ qua SĐT: 0354409959 để được hỗ trợ ! <br/>" +
                    "Thời gian đặt hàng： " + DateTime.Now.ToString("yyyy-MM-dd"));
                //emailMessage.Attachments.AddFileAttachment();
                foreach (var ele in emailAccounts)
                {
                    emailMessage.ToRecipients.Add(ele);
                    emailMessage.Send();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

    }
}
