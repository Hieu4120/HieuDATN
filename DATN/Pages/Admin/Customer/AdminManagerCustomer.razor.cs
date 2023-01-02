using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using static DATN.Services.NotificationServices;

namespace DATN.Pages.Admin.Customer
{
    public partial class AdminManagerCustomer
    {
        [Inject]
        private ICustomerServices icss { get; set; }
        private IEnumerable<m_customer> customers;
        private bool isLoading;
        private int ROW_INDEX = 1;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            customers = await icss.GetAllCustomer();
            isLoading = false;
            StateHasChanged();
        }
    }
}
