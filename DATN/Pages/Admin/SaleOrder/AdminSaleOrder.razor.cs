using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;


namespace DATN.Pages.Admin.SaleOrder
{
    public partial class AdminSaleOrder
    {
        [Inject]
        private ISaleOrderServices? isos { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private string CurrentUri = "manager-sale-order";
        private IEnumerable<m_sale_order>? sale_orders;
        private IEnumerable<m_sale_order> sale_orders_p = Enumerable.Empty<m_sale_order>();
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

        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }
        public async void CreatePagingInfo()
        {
            int PageSize = 4;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = sale_orders.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            sale_orders_p = sale_orders.Skip(skip).Take(PageSize).ToList();
        }

        /* private void UpdateSaleOrder(m_sale_order ele)
         {
             string sale_order_id = ele.sale_order_id.ToString();
             Dictionary<string, string> passData = new Dictionary<string, string>
             {
                 {"sale_order_id", sale_order_id},
             };
             iredir.RedirectParameter("sale-order-update", passData);
         }*/
    }
}
