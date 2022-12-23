﻿using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using System.Security.Claims;
using static DATN.Services.NotificationServices;

namespace DATN.Pages
{
    public partial class Home
    {
        [Inject]
        private IBookServices bs { get; set; }
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private ICartServices ics { get; set; }
        [Inject]
        private IAccountServices ias { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private ICarouselSevices icas { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        private IEnumerable<m_book>? books;
        private IEnumerable<m_carosel>? carosels;
        private m_cart? cart_item = new m_cart();
        private m_account? account_item = new m_account();

        private int cart_id_init;
        private string user;
        private bool CartItemIsExits;
        protected override async Task OnInitializedAsync()
        {
            carosels = await icas.GetAllCarousel();
            books = await bs.GetAllBook();
            StateHasChanged();
        }

        private async void pass_data_book(m_book ele)
        {
            ele.number_click += 1;
            await bs.Update(ele);
            Task.Delay(100);
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
            if (user == null )
            {
                ino.Notify((NotificationSeverity.Success, "Bạn vẫn chưa đăng nhập"));
                return;
            }
            else
            {
                if(CartItemIsExits == true)
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
