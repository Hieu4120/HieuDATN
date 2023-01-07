using BlazorStrap;
using DATN.Model;
using DATN.Services;
using DATN.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Carousel
{
    public partial class AdminUpdateCarousel
    {
        [Inject]
        private ICarouselSevices icas { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
      
        private m_carosel carosel_item = new m_carosel();
        private ModalConfirm? conf { set; get; }
        private bool isLoading = false;
        private int get_carousel_id;
        Regex regexNumberonly = new Regex("^[0-9]+$");
       /* public byte[] ImgUploaded { get; set; }
        //Limit file size = 25MB
        private long maxFileSize = 25 * 1048576;
        //Limit file = 1
        private int maxAllowedFiles = 1;*/
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("carosel_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_carousel_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-carousel");
                    return;
                }
            }
            bool CHK_get_carousel_id = await icas.ExistCarousel(get_carousel_id);
            if (!CHK_get_carousel_id)
            {
                iredir.RedirectNormal("manager-carousel");
                return;
            }
            carosel_item = await icas.GetCarouselById(get_carousel_id);      
            StateHasChanged();
        }
       /*private async Task HandleFileSelected(InputFileChangeEventArgs files)
        {
            foreach (var file in files.GetMultipleFiles(maxAllowedFiles))
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(maxFileSize).CopyToAsync(ms);
                ImgUploaded = ms.ToArray();
            }
        }*/
        private async void btn_handle_del_carousel()
        {
            conf.Show();
        }
        private async void call_back_delete_carousel()
        {
            conf.Close();
            await icas.Delete(carosel_item);
            iredir.RedirectNormal("manager-carousel");
        }

        private async Task UpdateCarousel()
        {
            isLoading = true;
            carosel_item.create_at = DateTime.Now;
            //carosel_item.carosel_image = ImgUploaded;
            await icas.Update(carosel_item);
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            iredir.RedirectNormal("manager-carousel");
            isLoading = false;
            StateHasChanged();
        }
    }
}
