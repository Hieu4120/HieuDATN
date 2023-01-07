using DATN.Services;
using DATN.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using DATN.Shared.Components;
using Radzen;
using static DATN.Services.NotificationServices;
using Microsoft.AspNetCore.Components.Forms;

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
        private m_genre? genre_item = new m_genre();
        private m_supplier? supplier_item = new m_supplier();
        private ModalConfirm? conf { set; get; }
        private bool isLoading = false;
        private int get_book_id;
  
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
                get_book_id = Int32.Parse(param1.First());
            }
            if (get_book_id == null || get_book_id == 0  )
            {
                iredir.RedirectNormal("manager-book");
                return;
            }
            book_item = await bs.GetBookById(get_book_id);
            genre_item = await ges.GetById(book_item.genre_id);
            supplier_item = await sus.GetSuppById(book_item.supplier_id);
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

