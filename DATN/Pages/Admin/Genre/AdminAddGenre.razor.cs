using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Genre
{
    public partial class AdminAddGenre
    {
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private INotificationService ino { get; set; }     
       
        private bool isLoading = false;
        private int genre_id_init;
        private bool IsName_genExist = false;
        private string errCodeStyle => (IsName_genExist) ? "outline: 1px solid red!important;" : "";
        private string errMessage = "Tên đã tồn tại";

        private m_genre genre = new m_genre();
        private List<string> status_gen = new List<string>()
        {
            "Hoạt động",
            "Chưa hoạt động"
        };

        private async void AddGenre()
        {
            bool isDouble = await ges.ExistName(genre.genre_name);
            if (isDouble)
            {
                IsName_genExist = true;
                return;
            }
            isLoading = true;
            genre_id_init = await ges.GetGenId();
            genre.create_at = DateTime.Now;
            genre.update_at = DateTime.Now;
            await ges.Create(genre);
            genre = new m_genre();
            isLoading = false;
            ino.Notify((NotificationSeverity.Success, "Thêm thành công"));
            StateHasChanged();
        }
    }
}
