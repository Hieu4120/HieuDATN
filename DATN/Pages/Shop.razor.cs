using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class Shop
    {
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IAccountServices ias { get; set; }
        [Inject]
        private IGenreServices igs { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Parameter]
        public int page { get; set; }
        private bool IsLoading;
        private string CurrentUri = "shop";
        PagingInfo pagingInfo = new PagingInfo();
        private IEnumerable<mediate_book>? books_i;
        private IEnumerable<mediate_genre>? genres;
        private m_cart? cart_item = new m_cart();
        private m_account? account_item = new m_account();
          private m_book? book_item;
        public IEnumerable<mediate_book> books { get; set; } = Enumerable.Empty<mediate_book>();
        private int cart_id_init;
        private string user;
        private bool CartItemIsExits;
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await Task.Delay(500);
            books_i = await bs.GetBookPriceFilter();
            genres = await igs.GetAllGenreName();
            IsLoading = false;
            StateHasChanged();
        }

        private async void getvaluecheck(ChangeEventArgs e, int index)
        {
            string check = e.Value.ToString();
            if (check == "on" && index == 1)
            {
                books = books_i;
            }
            if (check == "on" && index == 2)
            {
                books = books_i.Where(col => col.price < 100000).ToList();
            }
            if (check == "on" && index == 3)
            {
                books = books_i.Where(col => col.price > 100000 &&
                col.price < 300000).ToList();
            }
            if (check == "on" && index == 4)
            {
                books = books_i.Where(col => col.price > 300000 &&
                col.price < 500000).ToList();
            }
            if (check == "on" && index == 5)
            {
                books = books_i.Where(col => col.price > 500000).ToList();
            }
        }
        private async void GetGenreValue(ChangeEventArgs e)
        {
            int gen_check = Int32.Parse((string)e.Value);
            books = books_i.Where(col => col.genre_id == gen_check).ToList();
        }
        private async void pass_data_book(int book_id)
        {
            book_item = await bs.GetBookById(book_id);
            book_item.number_click += 1;
            await bs.Update(book_item);
            Task.Delay(200);
            string book_id_pass = book_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"book_id", book_id_pass},
            };
            iredir.RedirectParameter("detail", passData);
        }
        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }
        public async void CreatePagingInfo()
        {
            int PageSize = 6;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = books_i.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            books = books_i.Skip(skip).Take(PageSize).ToList();
        }
        private async void icon_add_to_cart(int book_id)
        {
            var authState = await authenticationStateTask;
            user = authState.User.Identity.Name;
            CartItemIsExits = await ics.ExistCartItemCHK(book_id);
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
                cart_item.book_id = book_id;
                cart_item.create_at = DateTime.Now;
                cart_item.update_at = DateTime.Now;
                await ics.Create(cart_item);
                ino.Notify((NotificationSeverity.Success, "Đã thêm vào giỏ hàng"));
            }
            StateHasChanged();
        }
    }
}
