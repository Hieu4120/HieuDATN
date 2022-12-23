using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class BookResultSearch
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private IAccountServices ias { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private string searchString;
        private int cart_id_init;
        private string user;
        private bool CartItemIsExits;
        private IEnumerable<m_book>? bookSearch;
        private m_cart? cart_item = new m_cart();
        private m_account? account_item = new m_account();
        protected override async Task OnInitializedAsync()
        {
            
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("search_string", out var param1))
            {
                searchString = param1.First();
            }
            bookSearch = await bs.GetBookByName(searchString);
            await Task.Delay(500);
            StateHasChanged();
        }
        private void pass_data_book(m_book ele)
        {
            string book_id = ele.book_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"book_id", book_id},
            };
            iredir.RedirectParameter("detail", passData);
        }

        private async void icon_add_to_cart(m_book ele)
        {
            var authState = await authenticationStateTask;
            user = authState.User.Identity.Name;
            CartItemIsExits = await ics.ExistCartItemCHK(ele.book_id);
            if (user == null)
            {
                ino.Notify((NotificationSeverity.Success, "Bạn vẫn chưa đăng nhập"));
                return;
            }
            else
            {
                if (CartItemIsExits == true)
                {
                    ino.Notify((NotificationSeverity.Success, "Sản phẩm đã có trong giỏ hàng"));
                    return;
                }
                account_item = await ias.GetCurrentCustomerByName(user);
                if (cart_item.cart_id != null)
                {
                    cart_id_init = await ics.GetCartId();
                }
                else
                {
                    cart_id_init = 1;
                }
                cart_item.cart_id = cart_id_init + 1;
                cart_item.customer_id = account_item.customer_id;
                cart_item.amount = 1;
                cart_item.book_id = ele.book_id;
                cart_item.create_at = DateTime.Now;
                cart_item.update_at = DateTime.Now;
                await ics.Create(cart_item);
                ino.Notify((NotificationSeverity.Success, "Đã thêm vào giỏ hàng"));
            }
            StateHasChanged();
        }
    }
}
