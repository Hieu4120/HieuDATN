using BlazorStrap;
using DATN.Model;
using DATN.Services;
using DATN.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages.Admin.Genre
{
    public partial class AdminUpdateGenre
    {
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IBookServices ibs { get; set; }
        private m_genre gen_item = new m_genre();
        private ModalConfirm? conf { set; get; }
        private bool isLoading = false;
        private string status_gen = "";
        private string? old_gen_name;
        private bool IsName_genExist = false;
        private bool IsGenreActive = false;
        Regex regexNumberonly = new Regex("^[0-9]+$");
        private string errCodeStyle => (IsName_genExist) ? "outline: 1px solid red!important;" : "";
        private string errMessage = "Tên đã tồn tại";

        private List<string> status = new List<string>()
        {
            "Hoạt động",
            "Chưa hoạt động"
        };
        private int get_gen_id;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("genre_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_gen_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-genre");
                    return;
                }
            }
            bool CHK_get_gen_id = await ges.CHKExistGenres(get_gen_id);
            if (!CHK_get_gen_id)
            {
                iredir.RedirectNormal("manager-genre");
                return;
            }
            gen_item = await ges.GetById(get_gen_id);
            old_gen_name = gen_item.genre_name;
            IsGenreActive = await ibs.ExistGenres(gen_item.genre_id);
        }

        private async Task UpdateGenre()
        {
            bool isDouble = await ges.ExistName(gen_item.genre_name);
            if ((old_gen_name != gen_item.genre_name) && isDouble)
            {
                IsName_genExist = true;
                return;
            }
            isLoading = true;
            gen_item.update_at = DateTime.Now;
            await ges.Update(gen_item);
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            isLoading = false;
            iredir.RedirectNormal("manager-genre");
            StateHasChanged();
        }

        private async void btn_handle_del_genre()
        {
            conf.Show();
        }

        private async void call_back_delete()
        {
            conf.Close();
            await ges.Delete(gen_item);
            ino.Notify((NotificationSeverity.Success, "Xóa thành công"));
            iredir.RedirectNormal("manager-genre");
        }
    }
}
