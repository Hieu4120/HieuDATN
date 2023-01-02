using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Carousel
{
    public partial class AdminAddNewCarousel
    {
        [Inject]
        private ICarouselSevices icas { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        private m_carosel carosel = new m_carosel();
        /*private string errCodeStyle => (IsName_genExist) ? "outline: 1px solid red!important;" : "";*/
        private bool isLoading = false;
        public byte[] ImgUploaded { get; set; }
        //Limit file size = 25MB
        private long maxFileSize = 25 * 1048576;
        //Limit file = 1
        private int maxAllowedFiles = 1;
        private bool IsNull = false;
        private string? errCodeStyle => IsNull ? "width: 100%; border:1px solid red!important;" : "width: 100%;";
        private string? errMess => IsNull ? "Vui lòng nhập hình ảnh" : "";
        async Task HandleFileSelected(InputFileChangeEventArgs files)
        {
            foreach (var file in files.GetMultipleFiles(maxAllowedFiles))
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(maxFileSize).CopyToAsync(ms);
                ImgUploaded = ms.ToArray();
            }
        }

        private async void AddCarousel()
        {
            if (ImgUploaded == null)
            {
                IsNull = true;
            }
            else
            {
                isLoading = true;
                carosel.create_at = DateTime.Now;
                carosel.carosel_image = ImgUploaded;
                await icas.Create(carosel);
                carosel = new m_carosel();
                isLoading = false;
                ino.Notify((NotificationSeverity.Success, "Thêm thành công"));
                StateHasChanged();
            }
        }
    }
}
