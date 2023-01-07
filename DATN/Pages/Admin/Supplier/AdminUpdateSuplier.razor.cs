using BlazorStrap;
using DATN.Model;
using DATN.Services;
using DATN.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using System.Text.RegularExpressions;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Supplier
{
    public partial class AdminUpdateSuplier
    {
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        [Inject]
        private IBookServices ibs { get; set; }
        private m_supplier sup_item = new m_supplier();
        private ModalConfirm? conf { set; get; }
        private bool isLoading = false;
        private string status_sup = "";
        private int get_sup_id;
        private string? old_sup_name;
        private bool IsSuppActive = false;
        private bool IsName_supExist = false;
        Regex regexNumberonly = new Regex("^[0-9]+$");
        private string errCodeStyle => (IsName_supExist) ? "outline: 1px solid red!important;" : "";
        private string errMessage = "Tên đã tồn tại";


        private List<string> status_supplier = new List<string>()
        {
            "Hoạt động",
            "Chưa hoạt động"
        };

        protected override async Task OnInitializedAsync()
        {
            var uri = iredir.GetUri();
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("supplier_id", out var param1))
            {
                if (regexNumberonly.IsMatch(param1.First()))
                {
                    get_sup_id = Int32.Parse(param1.First());
                }
                else
                {
                    iredir.RedirectNormal("manager-suplier");
                    return;
                }
            }
            bool CHK_get_sup_id = await sups.CHKExistSupp(get_sup_id);
            if (!CHK_get_sup_id)
            {
                iredir.RedirectNormal("manager-suplier");
                return;
            }
            sup_item = await sups.GetSuppById(get_sup_id);
            old_sup_name = sup_item.supplier_name;
            IsSuppActive = await ibs.ExistSupp(sup_item.supplier_id);
        }

        private async Task UpdateSup()
        {
            bool isDouble = await sups.ExistName(sup_item.supplier_name);
            if ((old_sup_name != sup_item.supplier_name) && isDouble)
            {
                IsName_supExist = true;
                return;
            }
            isLoading = true;
            sup_item.update_at = DateTime.Now;
            await sups.Update(sup_item);
            ino.Notify((NotificationSeverity.Success, "Cập nhật thành công"));
            isLoading = false;
            iredir.RedirectNormal("manager-suplier");
            StateHasChanged();
        }

        private async void btn_handle_del_supplier()
        {
            conf.Show();
        }

        private async void call_back_delete_sup()
        {
            conf.Close();
            await sups.Delete(sup_item);
            ino.Notify((NotificationSeverity.Success, "Xóa thành công"));
            iredir.RedirectNormal("manager-suplier");
        }
    }
}
