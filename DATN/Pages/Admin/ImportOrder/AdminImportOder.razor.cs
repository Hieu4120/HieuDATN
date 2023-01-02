using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DATN.Pages.Admin.ImportOrder
{
    public partial class AdminImportOder
    {
        [Inject]
        private IImportOrderServices? ipos { get; set; }
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private string CurrentUri = "manager-import-order";
        private IEnumerable<m_import_order>? import_Orders;
        private IEnumerable<m_import_order> import_Orders_p = Enumerable.Empty<m_import_order>();
        private List<m_import_order> import_order_List = new List<m_import_order>();
        private IEnumerable<m_supplier> supplier_item;
        private List<int> List_Suppid = new List<int>();
        private int ROW_INDEX = 1;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            import_Orders = await ipos.GetAllImportOrder();
            List_Suppid = import_Orders.Select(col => col.supplier_id).ToList();
            supplier_item = await sups.GetSuppByListId(List_Suppid);
            isLoading =false;
            StateHasChanged();
        }

        private void btn_pass_data_b(m_import_order ele)
        {
            string import_order_id = ele.import_order_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"import_order_id", import_order_id},
            };
            iredir.RedirectParameter("import-order-detail", passData);
        }
        private async void btn_export_excel()
        {
            foreach (var m in import_Orders)
            {
                import_order_List.Add(m);
            }
            var exService = new ExcelProcessSevices();
            string base64str = await exService.ExportExcelImportorder(import_order_List, sups);
            // export ex
            bool IsDisableCopy = await JS.InvokeAsync<bool>("ExcelExport", "ExcelExport.xlsx", base64str);
            StateHasChanged();
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
            pagingInfo.TotalItems = import_Orders.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            import_Orders_p = import_Orders.Skip(skip).Take(PageSize).ToList();
        }
    }
}
