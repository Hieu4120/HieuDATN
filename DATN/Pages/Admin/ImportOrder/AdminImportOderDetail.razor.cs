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

        private IEnumerable<m_import_order_detail>? import_order_detail;
        private List<m_import_order_detail>? import_order_detail_list =  new List<m_import_order_detail>() { };

        private m_genre? genres = new m_genre();
        private List<m_genre> genre_list = new List<m_genre>() { };
        private m_supplier? suppliers = new m_supplier();

        private int get_import_id;
        private int get_supp_id;
        private bool isLoading;
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
            import_order_detail = await imods.GetImportDetailByImportOrderId(get_import_id);
            foreach(var m in import_order_detail)
            {
                genres = await ges.GetById(m.genre_id);
                import_order_detail_list.Add(m);
                genre_list.Add(genres);
            }
            get_supp_id = import_order_detail.FirstOrDefault().supplier_id;
            suppliers = await sups.GetSuppById(get_supp_id);
            
            isLoading = false;
            StateHasChanged();
        }
    }
}
