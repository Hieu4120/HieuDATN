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

        private string CurrentUri = "shop";
        PagingInfo pagingInfo = new PagingInfo();
        private IEnumerable<m_book>? books_i;
        private IEnumerable<m_genre>? genres;
        private List<m_book>? booksFillter = new List<m_book>();
        private List<m_genre>? genre_list = new List<m_genre>();
        private m_cart? cart_item = new m_cart();
        private m_account? account_item = new m_account();
        public IEnumerable<m_book> books { get; set; } = Enumerable.Empty<m_book>();
        private int cart_id_init;
        private string user;
        private bool CartItemIsExits;
        protected override async Task OnInitializedAsync()
        {
            books_i = await bs.GetAllBook();
            await Task.Delay(500);
            genres = await igs.GetAllGenre();
            foreach (var book in books_i)
            {
                booksFillter.Add(book);
            }
            foreach(var item in genres)
            {
                genre_list.Add(item);
            }
           StateHasChanged();
        }

        private async void getvaluecheck( ChangeEventArgs e, int index)
        {
            string check = e.Value.ToString();
            if (check == "on" && index == 1)
            {
                books = booksFillter;
            }
            if (check == "on" && index == 2)
            {
                books = booksFillter.Where(col => col.price < 100000).ToList();
            }
            if (check == "on" && index == 3)
            {
                books = booksFillter.Where(col => col.price > 100000 &&
                col.price < 300000).ToList();
            }
            if (check == "on" && index == 4)
            {
                books = booksFillter.Where(col => col.price > 300000 &&
                col.price < 500000).ToList();
            }
            if (check == "on" && index == 5)
            {
                books = booksFillter.Where(col => col.price > 500000).ToList();
            }
        }
        private async void GetGenreValue(ChangeEventArgs e)
        {
            int gen_check = Int32.Parse((string)e.Value);
            books = booksFillter.Where(col => col.genre_id == gen_check).ToList();
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
        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }
        public async void CreatePagingInfo()
        {
            int PageSize = 3;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = books_i.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            books = books_i.Skip(skip).Take(PageSize).ToList();
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
