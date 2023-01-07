using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Radzen;
using System.Linq;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.ImportOrder
{
    public partial class AdminAddImportOrder
    {
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IImportOrderDetailServices imods { get; set; }
        [Inject]
        private IImportOrderServices imos { get; set; }

        private List<m_import_order_detail> import_order_detail = new List<m_import_order_detail>() { };
        private m_import_order import_order = new m_import_order();
        private IEnumerable<m_genre> genres = new List<m_genre>();
        private IEnumerable<m_supplier> suppliers = new List<m_supplier>();
        List<string> active_form_ind = new List<string>();
        int item_ind = -1;
        private int im_order_detail_id_init;
        private int supp_id;
        private bool isLoading = false;
        private int? Import_order_id { get; set; }
/*        private int form_number = 0;*/
        private DateTime? recive_date { get; set; }
        private decimal total_price = 0;
        public byte[] ImgUploaded { get; set; }

        //Limit file size = 25MB
        private long maxFileSize = 25 * 1048576;
        //Limit file = 1
        private int maxAllowedFiles = 1;
        protected override async Task OnInitializedAsync()
        {
            genres = await ges.GetAllGenre();
            suppliers = await sups.GetAllSupp();
        }

        private async void OnSelGenre(ChangeEventArgs e, int curr_form)
        {
            int genre_id = Int32.Parse((string)e.Value);
            import_order_detail[curr_form].genre_id = genre_id;
        }
        private async void OnSelSupp(ChangeEventArgs e)
        {
            supp_id = Int32.Parse((string)e.Value);
        }

        private void SelectItems(int index_form)
        {
            item_ind = index_form;
            for (int i = 0; i < import_order_detail.Count(); i++)
            {
                if (i == item_ind)
                {
                    active_form_ind[item_ind] = "background-color: #FFD333;";
                    continue;
                }
                active_form_ind[i] = "";
            }
        }
        private async Task HandleFileSelected(InputFileChangeEventArgs files, int index)
        {
            foreach (var file in files.GetMultipleFiles(maxAllowedFiles))
            {
                MemoryStream ms = new MemoryStream();

                await file.OpenReadStream(maxFileSize).CopyToAsync(ms);
                ImgUploaded = ms.ToArray();
                import_order_detail[index].book_image = ImgUploaded;
            }
        }
        private async void btn_add_new()
        {
            if (import_order_detail.Count() >= 5)
            {
                return;
            }
            import_order_detail.Add(new m_import_order_detail() { });
            active_form_ind.Add("");
            StateHasChanged();
        }
        private async void btn_remove()
        {
            if (item_ind <= -1)
            {
                ino.Notify((NotificationSeverity.Success, "Vui lòng chọn form xóa"));
                return;
            }
            import_order_detail.Remove(import_order_detail[item_ind]);
            active_form_ind.Remove(active_form_ind[item_ind]);
            item_ind = -1;
            StateHasChanged();
        }

        private async void btn_add_import_order()
        {
            
            foreach (var ele in  import_order_detail)
            {
                var CHK = import_order_detail.Where(col => col.book_id == ele.book_id).ToList();
                if(CHK.Count() != 1)
                {
                    ino.Notify((NotificationSeverity.Success, $"ID sách đã tồn tại {ele.book_id}"));
                    return;
                }

                if (import_order?.import_order_id == null || import_order?.supplier_id == null ||
                    import_order?.receive_at == null || ele?.book_id == null ||
                    ele.book_name == null || ele.book_image == null ||
                    ele?.genre_id == null || ele.author == null ||
                    ele.price == null || ele.amount == null ||
                    ele.page_number == null)
                {
                    ino.Notify((NotificationSeverity.Success, "Làm ơn điền đầy đủ thông tin"));
                    return;
                }
                
                bool isDouble = await imods.CheckBookNameExist(ele.book_name);
                if (isDouble)
                {
                    ino.Notify((NotificationSeverity.Success, "Tên sách đã tồn tại:" + ele.book_name));
                    return;
                }
                bool isDouble_id = await imods.CheckBookIdExist((int)ele.book_id);
                if (isDouble_id)
                {
                    ino.Notify((NotificationSeverity.Success, "ID sách đã tồn tại:" + ele.book_id));
                    return;
                }
                
                bool isDouble_import = await imods.CheckImportOrderId((int)Import_order_id);
                if (isDouble_import)
                {
                    ino.Notify((NotificationSeverity.Success, "ID Đơn hàng đã tồn tại"));
                    return;
                }
                if (im_order_detail_id_init != null)
                {
                    im_order_detail_id_init = await imods.GetId();
                }
                else
                {
                    im_order_detail_id_init = 1;
                }
                ele.import_order_detail_id = im_order_detail_id_init + 1;
                ele.create_at = DateTime.Now;
                ele.update_at = DateTime.Now;
                ele.supplier_id = supp_id;
                ele.import_order_id = (int)Import_order_id;
                total_price = ((decimal)(total_price + ele.price));
            }
            isLoading = true;
            await imods.CreateRange(import_order_detail);

            import_order.supplier_id = supp_id;
            import_order.import_order_id = (int)Import_order_id;
            import_order.price = total_price;
            import_order.create_at = DateTime.Now;
            import_order.receive_at = (DateTime)recive_date;
            await imos.Create(import_order);
            ino.Notify((NotificationSeverity.Success, "Thêm đơn hàng thành công"));
            await Task.Delay(100);
            import_order_detail = new List<m_import_order_detail>() { };
            Import_order_id = null;
            //suppliers = new List<m_supplier>();
            recive_date = null;
            isLoading = false;
            StateHasChanged();
        }
        /*public void validCheckAll()
        {
            foreach (var ele in import_order_detail)
            {
                
            }
        }*/
    }
}
