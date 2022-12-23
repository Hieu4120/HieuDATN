using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.SaleOrder
{
    public partial class AdminSaleOrderUpdate
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private ISaleOrderServices? isos { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        private m_sale_order sale_Order = new m_sale_order();
        private bool isLoading = false;
        private List<string> status_sale = new List<string>()
        {
            "Hoàn thành",
            "Chưa hoàn thành"
        };
        private int get_sale_id;
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sale_order_id", out var param1))
            {
                get_sale_id = Int32.Parse(param1.First());
            }
            if (get_sale_id == null || get_sale_id == 0)
            {
                iredir.RedirectNormal("manager-sale-order");
                return;
            }
            sale_Order = await isos.GetById(get_sale_id);
        }

        private async void UpdateSaleorder()
        {
            isLoading = true;
            
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            await isos.Update(sale_Order);
            isLoading = false;
            iredir.RedirectNormal("manager-sale-order");
            StateHasChanged();
        }
    }
}
