using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using static DATN.Services.NotificationServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages.Admin.ImportOrder
{
    public partial class AdminImportOderDetail
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IImportOrderDetailServices imods { get; set; }
        [Parameter]
        public int page { get; set; }

        private IEnumerable<mediate_import_order_detail>? import_order_detail;
        private IEnumerable<mediate_import_order_detail>? import_order_detail_p = Enumerable.Empty<mediate_import_order_detail>();
        private int get_import_id;
        private string sup_name;
        private bool isLoading;
        private string CurrentUri = "import-order-detail";
        PagingInfo pagingInfo = new PagingInfo();
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("import_order_id", out var param1))
            {
                get_import_id = Int32.Parse(param1.First());
            }
            if (get_import_id == null || get_import_id == 0)
            {
                iredir.RedirectNormal("manager-import-order");
                return;
            }
            isLoading = true;
            import_order_detail = await imods.GetImportDetailByImportOrderId1(get_import_id);
            sup_name = import_order_detail.Select(col => col.supplier_name).First();
            isLoading = false;
            StateHasChanged();
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
            pagingInfo.TotalItems = import_order_detail.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            import_order_detail_p = import_order_detail.Skip(skip).Take(PageSize).ToList();
        }
    }
}
