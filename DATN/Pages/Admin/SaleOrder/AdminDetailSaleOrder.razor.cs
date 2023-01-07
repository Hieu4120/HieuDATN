using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages.Admin.SaleOrder
{
    public partial class AdminDetailSaleOrder
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private ISaleOrderDetailServices? isods { get; set; }
        [Inject]
        private ISaleOrderServices? isos { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IBookServices? ibs { get; set; }
        [Inject]
        private IImportOrderDetailServices? iimps { get; set; }
        private int get_sale_id;
        private bool isLoading;
        private int ROW_INDEX = 1;
        Regex regexNumberonly = new Regex("^[0-9]+$");

        private IEnumerable<m_sale_order_detail>? sale_order_detail;
        private IEnumerable<m_book>? books;
        private List<m_book>? booklist = new List<m_book>();
        private IEnumerable<m_import_order_detail>? import_Order_Details;
        private List<m_import_order_detail>? import_Order_Details_List = new List<m_import_order_detail>();
        private Dictionary<string, int?> listamount = new Dictionary<string, int?>();
        private m_sale_order sale_Order = new m_sale_order();
        private List<string> status_sale = new List<string>()
        {
            "Hoàn thành",
            "Chưa hoàn thành"
        };
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sale_order_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_sale_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-sale-order");
                    return;
                }
            }
            bool CHK_get_book_id = await isods.ExistSaleOrder(get_sale_id);
            if (!CHK_get_book_id)
            {
                iredir.RedirectNormal("manager-sale-order");
                return;
            }
            isLoading = true;
            sale_Order = await isos.GetById(get_sale_id);
            sale_order_detail = await isods.GetSaleOrderDetailBySaleOrderId(get_sale_id);
            foreach(var item in sale_order_detail)
            {
                listamount.Add(item.book_name, item.amount);
            }
            var list_name = sale_order_detail.Select(col => col.book_name).ToList();
            books = await ibs.GetBookByListName(list_name);
            var list_id_book = books.Select(col => col.book_id).ToList();
            import_Order_Details = await iimps.GetImportDetailByListIdBook(list_id_book);
            isLoading = false;
            StateHasChanged();
        }

        private async void UpdateSaleorder()
        {
            isLoading = true;
            if(sale_Order.status.Equals("Hoàn thành"))
            {
                foreach(var item in books)
                {
                    item.amount = item.amount - listamount[item.book_name];
                    booklist.Add(item);
                }
                await ibs.Updaterange(booklist);
                foreach(var ele in import_Order_Details)
                {
                    ele.amount = ele.amount - listamount[ele.book_name];
                    import_Order_Details_List.Add(ele);
                }
                await iimps.UpdateRange(import_Order_Details_List);
            }
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            await isos.Update(sale_Order);
            isLoading = false;
            iredir.RedirectNormal("manager-sale-order");
            StateHasChanged();
        }
    }
}
