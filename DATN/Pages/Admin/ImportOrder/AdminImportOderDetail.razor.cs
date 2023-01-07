using BlazorStrap;
using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace DATN.Pages.Admin.ImportOrder
{
    public partial class AdminImportOderDetail
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IImportOrderDetailServices imods { get; set; }
        [Parameter]
        public int page { get; set; }

        private IEnumerable<mediate_import_order_detail>? import_order_detail;
        private int get_import_id;
        private string sup_name;
        private bool isLoading;
        Regex regexNumberonly = new Regex("^[0-9]+$");
        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("import_order_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_import_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-import-order");
                    return;
                }
            }
            bool CHK_get_import_id = await imods.ExistImportOrderDetail(get_import_id);
            if (!CHK_get_import_id)
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
    }
}
