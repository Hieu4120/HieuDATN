using DATN.Services;
using DATN.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using static DATN.Services.NotificationServices;
using DocumentFormat.OpenXml.Vml.Office;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages.Admin.Book
{
    public partial class AdminAddNewBook
    {
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IBookServices abs { get; set; }
        [Inject]
        private INotificationService? ino { get; set; }
        [Inject]
        private IImportOrderDetailServices? iipods { get; set; }

        private m_genre? genre;
        private m_supplier? supplier;
        private IEnumerable<m_import_order_detail>? import_order;
        private m_import_order_detail? buff_import_order;

        private bool isLoading = false;
        private bool isDisable = false;

        private int? GenId;
        private int? SuppId;
        private int? BookId { get; set; }
        private bool IsDisable = false;
        private string gen_name { get; set; }
        private string supp_name { get; set; }
        private string RetSearchValue = "";
        private m_book books = new m_book();
        public byte[] ImgUploaded { get; set; }
        private bool isLoadingAuto = false;
        //Limit file size = 25MB
        private long maxFileSize = 25 * 1048576;
        //Limit file = 1
        private int maxAllowedFiles = 1;
        private List<string> status_b = new List<string>()
        {
            "Mở bán",
            "Chưa mở bán"
        };
        protected override async Task OnInitializedAsync()
        {
            if (RetSearchValue == null || RetSearchValue == "")
            {
                IsDisable = true;
            }
        }
        /* async Task HandleFileSelected(InputFileChangeEventArgs files)
         {
             foreach (var file in files.GetMultipleFiles(maxAllowedFiles))
             {
                 MemoryStream ms = new MemoryStream();
                 await file.OpenReadStream(maxFileSize).CopyToAsync(ms);
                 ImgUploaded = ms.ToArray();
             }
         }*/
        private async void OnLoadData(LoadDataArgs args)
        {
            /* import_order = new List<m_import_order_detail>()
             {
                 new m_import_order_detail(),
             };*/
            isLoadingAuto = true;
            var _import_order = await iipods.search(args.Filter);
            import_order = _import_order.OrderBy(ele => ele.import_order_id);
            isLoadingAuto = false;
            await InvokeAsync(StateHasChanged);
        }
        private void OnChanged(object value)
        {
            RetSearchValue = "";
            if (import_order == null ||
                import_order.Count() == 0 ||
                import_order?.First().book_id == null)
            {
                RetSearchValue = (string)value;
                return;
            }
            RetSearchValue = (string)value;
            buff_import_order = import_order
                .Where(e => e.book_id.ToString()
                .Equals(RetSearchValue))
                .Distinct()
                .FirstOrDefault();
            BookId = buff_import_order?.book_id;
            if (buff_import_order == null)
            {
                return;
            }
            else
            {
                IsDisable = false;
                GenId = buff_import_order.genre_id;
                SuppId = buff_import_order.supplier_id;
                RetSearchValue = buff_import_order.book_id.ToString();
                RetSearchValue += " " + buff_import_order.book_name;
            }
            StateHasChanged();
        }

        private async void btn_click_copy()
        {
            if (BookId != null)
            {
                var CHKExist = await iipods.CheckBookIdExist((int)BookId);
                if (!CHKExist)
                {
                    buff_import_order = await iipods.GetImportDetailByBookId((int)BookId);
                    genre = await ges.GetById((int)GenId);
                    gen_name = genre.genre_name;
                    supplier = await sups.GetSuppById((int)SuppId);
                    supp_name = supplier.supplier_name;
                    books.book_id = (int)buff_import_order.book_id;
                    books.book_image = buff_import_order.book_image;
                    books.book_name = buff_import_order.book_name;
                    books.page_number = (int)buff_import_order.page_number;
                    books.amount = buff_import_order.amount;
                    books.author = buff_import_order.author;
                    books.price = buff_import_order.price;
                    books.genre_id = buff_import_order.genre_id;
                    books.supplier_id = buff_import_order.supplier_id;
                }
                else
                {
                    ino.Notify((NotificationSeverity.Success, "Sách đã tồn tại"));
                    return;
                }
            }
            else
            {
                ino.Notify((NotificationSeverity.Success, "Thiếu dữ liệu"));
                return;
            }
            StateHasChanged();
        }

        private async void AddBook()
        {
            isLoading = true;
            books.update_at = DateTime.Now;
            await abs.Create(books);
            books = new m_book();
            gen_name = "";
            supp_name = " ";
            isLoading = false;
            ino.Notify((NotificationSeverity.Success, "Thêm thành công"));
            StateHasChanged();

        }
    }
}
