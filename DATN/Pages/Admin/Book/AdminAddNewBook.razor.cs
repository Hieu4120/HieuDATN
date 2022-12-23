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
        private INotificationService ino { get; set; }
        [Inject]
        private IImportOrderDetailServices iipods { get; set; }

        private m_genre? genre { get; set; }
        private m_supplier? supplier { get; set; }
        private IEnumerable<m_import_order_detail> import_order { get; set; }

        private int book_id_init;
        private bool isLoading = false;
        private string? errMess = "";
        private string? errCodeStyle_n = "";
        private string? errCodeStyle_g = "";
        private string? errCodeStyle_s = "";
        private string? errCodeStyle_a = "";
        private string? errCodeStyle_p = "";
        private string? errCodeStyle_c = "";
        private string? errCodeStyle_t = "";
        private string? errCodeStyle_m = "";
        private string? errCodeStyle_i = "";
        private string? CSS_outline = "outline: 1px solid red!important;";

        private int? GenId;
        private int? SuppId;
        private int BookId;
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
            /* genres = await ges.GetAllGenre();
             suppliers = await sups.GetAllSupp();*/
        }
        async Task HandleFileSelected(InputFileChangeEventArgs files)
        {
            foreach (var file in files.GetMultipleFiles(maxAllowedFiles))
            {
                MemoryStream ms = new MemoryStream();
                await file.OpenReadStream(maxFileSize).CopyToAsync(ms);
                ImgUploaded = ms.ToArray();
            }
        }
        private async void selSupp(ChangeEventArgs e)
        {
            books.supplier_id = Int32.Parse((string)e.Value);
        }

        private async void selGen(ChangeEventArgs e)
        {
            books.genre_id = Int32.Parse((string)e.Value);
        }
        private async void OnLoadData(LoadDataArgs args)
        {
            // 初期化の値
            import_order = new List<m_import_order_detail>()
            {
                new m_import_order_detail(),
            };
            // 処理中
            isLoadingAuto = true;
            // 取得データ
            var _import_order = await iipods.search(args.Filter);
            import_order = _import_order.OrderBy(ele => ele.import_order_id);
            // 処理完了
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

            var buff_import_order = import_order
                .Where(e => e.book_id.ToString().Equals(RetSearchValue)).Distinct().FirstOrDefault();

            if (buff_import_order == null)
            {
                return;
            }
            else
            {
                BookId = (int)buff_import_order.book_id;
                GenId = buff_import_order.genre_id;
                SuppId = buff_import_order.supplier_id;
                RetSearchValue = buff_import_order.book_id.ToString();
                RetSearchValue += " " + buff_import_order.book_name;
            }
            StateHasChanged();
        }

        private async void btn_click_copy()
        {

            var _import_order = await iipods.GetImportDetailByBookId((BookId));
            if (_import_order != null)
            {
                genre = await ges.GetById((int)GenId);
                gen_name = genre.genre_name;
                supplier = await sups.GetSuppById((int)SuppId);
                supp_name = supplier.supplier_name;
                books.book_id = (int)_import_order.book_id;
                books.book_image = _import_order.book_image;
                books.book_name = _import_order.book_name;
                books.page_number = (int)_import_order.page_number;
                books.amount = _import_order.amount;
                books.author = _import_order.author;
                books.price = _import_order.price;
                books.genre_id = _import_order.genre_id;
                books.supplier_id = _import_order.supplier_id;
            }
            else
            {
                ino.Notify((NotificationSeverity.Success, "Thiếu dữ liệu"));
            }
            StateHasChanged();
        }

        private async void AddBook()
        {
            if (book_id_init != null)
            {
                book_id_init = await abs.GetId();
            }
            else
            {
                book_id_init = 1;
            }
            isLoading = true;

            //books.book_id = book_id_init + 1;
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
