using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Carousel
{
    public partial class AddminManagerCarousel
    {
        [Inject]
        private ICarouselSevices icas { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        private bool isLoading;
        private IEnumerable<m_carosel> carosels { get; set; }
        private int ROW_INDEX = 1;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            carosels = await icas.GetAllCarousel();
            isLoading = false;
            StateHasChanged();
        }

        private void btn_pass_data_carousel(m_carosel ele)
        {
            string carol_id = ele.carosel_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"carosel_id", carol_id},
            };
            iredir.RedirectParameter("update-carousel", passData);
        }
    }
}
