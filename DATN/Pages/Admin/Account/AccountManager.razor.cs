using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Account
{
    public partial class AccountManager
    {
        [Inject]
        private IAccountServices iacs { get; set; }
        private IEnumerable<m_account> accounts { get; set; }
        private int ROW_INDEX = 1;
        protected override async Task OnInitializedAsync()
        {
            accounts = await iacs.GetAllAcc();
            StateHasChanged();
        }
    }
}
