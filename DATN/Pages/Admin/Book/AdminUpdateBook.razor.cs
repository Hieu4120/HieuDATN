using DATN.Services;
using DATN.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using DATN.Shared.Components;
using Radzen;
using static DATN.Services.NotificationServices;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

namespace DATN.Pages.Admin.Book
{
    public partial class AdminUpdateBook
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IBookServices? bs { get; set; }
        [Inject]
        private ISupplierServices? sus { get; set; }
        [Inject]
        private IGenreServices? ges { get; set; }
        [Inject]
        private INotificationService ino { get; set; }

        private m_book? book_item = new m_book();
       /* private m_genre? genre_item = new m_genre();
        private m_supplier? supplier_item = new m_supplier();*/
        private ModalConfirm? conf { set; get; }
        private bool isLoading = false;
        private string genre_name;
        private string supplier_name;
        private int get_book_id;
        Regex regexNumberonly = new Regex("^[0-9]+$");

        private List<string> status_book_list = new List<string>()
        {
            "Mở bán",
            "Chưa mở bán"
        };
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("book_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_book_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-book");
                    return;
                }
            }
            bool CHK_get_book_id = await bs.ExistBook(get_book_id);
            if (!CHK_get_book_id)
            {
                iredir.RedirectNormal("manager-book");
                return;
            }
            book_item = await bs.GetBookById(get_book_id);
            genre_name = await ges.GetGenreNameById(book_item.genre_id);
            supplier_name = await sus.GetSuppNameById(book_item.supplier_id);
            StateHasChanged();
        }

        private async Task UpdateBook()
        {
            isLoading = true;
            book_item.update_at = DateTime.Now;
            await bs.Update(book_item);
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            iredir.RedirectNormal("manager-book");
            isLoading = false;
            StateHasChanged();
        }
        private async void btn_handle_del_book()
        {
            conf.Show();
        }
        private async void call_back_delete_book()
        {
            conf.Close();
            await bs.Delete(book_item);
            ino.Notify((NotificationSeverity.Success, "Xóa thành công"));
            iredir.RedirectNormal("manager-book");
        }
    }
}

