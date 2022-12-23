using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DATN.Pages.Admin.SaleOrder
{
    public partial class AdminSaleOrder
    {
        [Inject]
        private ISaleOrderServices? isos { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        private IEnumerable<m_sale_order> sale_orders;
        private int ROW_INDEX = 1;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            sale_orders = await isos.GetAllsaleOrder();
            isLoading = false;
            StateHasChanged();
        }

        private void btn_pass_data_b(m_sale_order ele)
        {
            string sale_order_id = ele.sale_order_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"sale_order_id", sale_order_id},
            };
            iredir.RedirectParameter("sale-order-detail", passData);
        }

        private void UpdateSaleOrder(m_sale_order ele)
        {
            string sale_order_id = ele.sale_order_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"sale_order_id", sale_order_id},
            };
            iredir.RedirectParameter("sale-order-update", passData);
        }
    }
}
