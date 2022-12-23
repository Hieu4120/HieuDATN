using DATN.Model;
using DATN.Services;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using static DATN.Services.NotificationServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages
{
    public partial class Cart
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        private IEnumerable<m_cart>? carts;
        private m_book? book_item = new m_book();
        private List<m_book>? List_books = new List<m_book>();
        private Dictionary<int, int>? Dic_total_price = new Dictionary<int, int>();

        private int get_cus_id;
        private int? total_price;
        private int? total;
        private int? ship_price = 20000;
        private int? total_pay;
        private List<bool> isDisable = new List<bool>();
        private int curr_amount = 1;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("customer_id", out var param1))
            {
                get_cus_id = Int32.Parse(param1.First());
            }
            await Task.Delay(20);
            carts = await ics.GetCartItembyCusId(get_cus_id);
            foreach (var ele in carts)
            {
                book_item = await bs.GetBookById(ele.book_id);
                total_price = (int)(book_item.price * ele.amount);
                Dic_total_price.Add(book_item.book_id, (int)total_price);
                List_books.Add(book_item);
                isDisable.Add(false);
            }
            total = Dic_total_price.Sum(x => x.Value);
            total_pay = total + ship_price;
            StateHasChanged();
        }
        private async void btn_minus(m_cart c_item, int index)
        {
            if (c_item.amount == 1)
            {
                isDisable[index] = true;
                isDisable.Add(isDisable[index]);
                return;
            }
            c_item.amount = c_item.amount - 1;
            c_item.update_at = DateTime.Now;
            await ics.Update(c_item);
            total_price = (int)(book_item.price * curr_amount);
            StateHasChanged();
        }

        private async void btn_plus(m_cart c_item, int index)
        {
            isDisable[index] = false;
            isDisable.Add(isDisable[index]);
            if (c_item.amount >= book_item.amount)
            {
                ino.Notify((NotificationSeverity.Success, "Số lượng tồn không đủ"));
                return;
            }
            c_item.amount = c_item.amount + 1;
            c_item.update_at = DateTime.Now;
            await ics.Update(c_item);
            total_price = (int)(book_item.price * curr_amount);
            StateHasChanged();
        }
        private void btn_reload()
        {
            iredir.RedirectNormalForce($"cart?customer_id={get_cus_id}", true);
        }

        private void Pass_Data()
        {
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"cus_id", get_cus_id.ToString()},
                {"total", total.ToString()}
            };
            iredir.RedirectParameter("checkout", passData);
        }
        private async void delete_cart_item(m_cart cart_item)
        {
            await ics.Delete(cart_item);
            await Task.Delay(100);
            carts = await ics.GetAllCart();
            StateHasChanged();
        }
    }
}
