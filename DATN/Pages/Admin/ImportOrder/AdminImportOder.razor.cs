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
        private IEnumerable<m_import_order> import_Orders;
        private List<m_import_order> import_order_List = new List<m_import_order>();
        private m_supplier? supplier_item { get; set; }
        private List<m_supplier>? suppliers = new List<m_supplier>();
        private int ROW_INDEX = 1;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            import_Orders = await ipos.GetAllImportOrder();
            foreach (var item in import_Orders)
            {
                supplier_item = await sups.GetSuppById(item.supplier_id);
                suppliers.Add(supplier_item);
            }
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
    }
}
