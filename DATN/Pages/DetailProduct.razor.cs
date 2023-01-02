using DATN.Model;
using DATN.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class DetailProduct
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
        private IReviewServices? irs { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private IAccountServices ias { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IReviewServices irews { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private IEnumerable<m_book>? book_similers;
        private IEnumerable<m_review>? reviews;

        private mediate_book_detail? book_Detail = new mediate_book_detail();
        private m_review? review_item = new m_review();
        private m_cart? cart_item = new m_cart();
        private m_cart? cart_item_exist = new m_cart();
        private m_account? account_item = new m_account();

        private bool isLoading = false;
        private bool isDisable = false;
        private bool CartItemExist = false;
        private int curr_amount = 1;
        private int cart_id_init;
        private int cart_id_init_icon;
        private string user;
        private bool CartItemIsExits;
        private List<int> ratings = new List<int>()
        {
            1,
            2,
            3,
            4,
            5
        };
        private int get_book_id;
        private int review_id_init;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("book_id", out var param1))
            {
                get_book_id = Int32.Parse(param1.First());
            }
            book_Detail = await bs.bookDetail(get_book_id);
            book_similers = await bs.GetBookByGenre(book_Detail.genre_id);
            reviews = await irews.GetReViewByBookId(get_book_id);
            var authState = await authenticationStateTask;
            user = authState.User.Identity.Name;
            StateHasChanged();
        }

        private async void SelChange(ChangeEventArgs e)
        {
            review_item.rating = Int32.Parse(e.Value.ToString());
        }

        private async void AddReview()
        {
            if (user == null)
            {
                ino.Notify((NotificationSeverity.Success, "Bạn vẫn chưa đăng nhập"));
                return;
            }
            else
            {
                if (review_id_init != null)
                {
                    review_id_init = await irs.GetReviewId();
                }
                else
                {
                    review_id_init = 1;
                }
                isLoading = true;
                review_item.create_at = DateTime.Now;
                review_item.update_at = DateTime.Now;
                review_item.book_id = get_book_id;
                review_item.review_id = review_id_init + 1;
                await irs.Create(review_item);
                review_item = new m_review();
                isLoading = false;
                ino.Notify((NotificationSeverity.Success, "Gửi thành công"));
            }
            StateHasChanged();
        }
        private async void btn_minus()
        {
            if (curr_amount == 1)
            {
                isDisable = true;
                return;
            }
            curr_amount = curr_amount - 1;
            StateHasChanged();
        }

        private async void btn_plus()
        {
            isDisable = false;
            if (curr_amount >= book_Detail.amount)
            {
                ino.Notify((NotificationSeverity.Success, "Số lượng tồn không đủ"));
                return;
            }
            curr_amount = curr_amount + 1;
            StateHasChanged();
        }
        private async void icon_add_to_cart(m_book ele)
        {

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
                    cart_id_init_icon = await ics.GetCartId();
                }
                else
                {
                    cart_id_init_icon = 1;
                }
                cart_item.cart_id = cart_id_init_icon + 1;
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
        private async void pass_data_book(m_book ele)
        {
            ele.number_click += 1;
            await bs.Update(ele);
            string book_id = ele.book_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"book_id", book_id},
            };
            iredir.RedirectParameterForce("detail", passData, true);
        }
        private async void btn_add_to_cart()
        {
            var authState = await authenticationStateTask;

            user = authState.User.Identity.Name;
            if (user == null)
            {
                ino.Notify((NotificationSeverity.Success, "Bạn vẫn chưa đăng nhập"));
                return;
            }
            else
            {
                account_item = await ias.GetCurrentCustomerByName(user);
                cart_item_exist = await ics.GetCartItembyBookId(get_book_id);
                int old_amount = cart_item_exist.amount;
                if (cart_item.cart_id != null)
                {
                    cart_id_init = await ics.GetCartId();
                }
                else
                {
                    cart_id_init = 1;
                }
                if (cart_item_exist != null && cart_item_exist.book_id == get_book_id)
                {

                    cart_item_exist.amount = curr_amount + old_amount;
                    cart_item_exist.update_at = DateTime.Now;
                    await ics.Update(cart_item_exist);
                    ino.Notify((NotificationSeverity.Success, "Đã thêm vào giỏ hàng"));
                }
                else
                {
                    cart_item.cart_id = cart_id_init + 1;
                    cart_item.customer_id = account_item.customer_id;
                    cart_item.amount = curr_amount;
                    cart_item.book_id = book_Detail.book_id;
                    cart_item.create_at = DateTime.Now;
                    cart_item.update_at = DateTime.Now;
                    await ics.Create(cart_item);
                    ino.Notify((NotificationSeverity.Success, "Đã thêm vào giỏ hàng"));
                }
            }
            StateHasChanged();
        }
    }
}

