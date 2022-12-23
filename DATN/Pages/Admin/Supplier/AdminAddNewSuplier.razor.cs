using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Supplier
{
    public partial class AdminAddNewSuplier
    {
        [Inject]
        private ISupplierServices sups { get; set; }
        [Inject]
        private INotificationService ino { get; set; }
        private bool isLoading = false;
        private bool IsName_supExist = false;
        private string errCodeStyle => (IsName_supExist) ? "outline: 1px solid red!important;" : "";
        private string errMessage = "Tên đã tồn tại";

        private int supplier_id_init;
        private string? old_supp_name;
        private m_supplier supp = new m_supplier();
        private List<string> status_sup = new List<string>()
        {
            "Hoạt động",
            "Chưa hoạt động"
        };
                          
        private async void AddSupp()
        {
            bool isDouble = await sups.ExistName(supp.supplier_name);
            if (isDouble)
            {
                IsName_supExist = true;
                return;
            }
            supplier_id_init = await sups.GetSupId();
            isLoading = true;
            supp.supplier_id = supplier_id_init + 1;
            supp.create_at = DateTime.Now;
            supp.update_at = DateTime.Now;
            await sups.Create(supp);
            supp = new m_supplier();
            isLoading = false;
            ino.Notify((NotificationSeverity.Success, "Thêm thành công"));
            StateHasChanged();
        }
    }
}
