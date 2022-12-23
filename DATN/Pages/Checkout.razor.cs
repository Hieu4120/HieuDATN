using DATN.Model;
using DATN.Services;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class Checkout
    {
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private ISaleOrderDetailServices isods { get; set; }
        [Inject]
        private ISaleOrderServices isos { get; set; }
        private m_sale_order_detail sale_order_detail = new m_sale_order_detail();
        private m_sale_order_detail sale_order_detail_buff = new m_sale_order_detail();
        private m_sale_order sale_order = new m_sale_order();
        private List<m_sale_order_detail> sale_order_detail_List = new List<m_sale_order_detail>();
        private int get_cus_id;
        private List<int> book_id_list = new List<int>();
        private IEnumerable<m_cart>? carts;
        private IEnumerable<m_book> book_list;
        private IEnumerable<m_mediate_checkout>? DataList;
        private int? total;
        private int? ship_price = 20000;
        private int? total_pay;
        private int sale_detail_id_init;
        private int sale_id_init;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("cus_id", out var param1))
            {
                get_cus_id = Int32.Parse(param1.First());
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("total", out var param2))
            {
                total = Int32.Parse(param2.First());
            }
            carts = await ics.GetCartItembyCusId(get_cus_id);
            await Task.Delay(20);
            DataList = await ics.GetCartItem(get_cus_id);
            total_pay = total + ship_price;
            StateHasChanged();
        }
        private async void check_out()
        {
            if (sale_id_init != null)
            {
                sale_id_init = await isods.GetId();
            }
            else
            {
                sale_id_init = 1;
            }
            foreach (var ele in DataList)
            {
                if (sale_detail_id_init != null)
                {
                    sale_detail_id_init = await isods.GetId();
                }
                else
                {
                    sale_detail_id_init = 1;
                }
                sale_order_detail_buff = new m_sale_order_detail();
                //sale_order_detail_buff.sale_order_detail_id = sale_detail_id_init + 1;
                sale_order_detail_buff.book_name = ele.book_name;
                sale_order_detail_buff.amount = ele.amount;
                sale_order_detail_buff.price = ele.price;
                sale_order_detail_buff.create_at = DateTime.Now;
                sale_order_detail_buff.sale_order_id = sale_id_init + 1;
                sale_order_detail_buff.first_name = sale_order_detail.first_name;
                sale_order_detail_buff.last_name = sale_order_detail.last_name;
                sale_order_detail_buff.phone_number = sale_order_detail.phone_number;
                sale_order_detail_buff.address = sale_order_detail.address;
                sale_order_detail_List.Add(sale_order_detail_buff);
            }
            await isods.CreateRange(sale_order_detail_List);
            sale_order.sale_order_id = sale_id_init + 1;
            sale_order.price = (decimal)total_pay;
            sale_order.status = "Chưa Hoàn thành";
            sale_order.purchase_date = DateTime.Now;
            await isos.Create(sale_order);
            await ics.DeleteRange((List<m_cart>)carts);
            StateHasChanged();
        }
    }
}
