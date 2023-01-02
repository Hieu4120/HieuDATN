using DATN.Pages;
using DATN.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;

namespace DATN.Shared
{
    public partial class NavMenu
    {
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Inject]
        private IAccountServices? acs { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private const string HOME = "HOME";
        private const string SHOP = "SHOP";
        private const string CONTACT = "CONTACT";
        private const string INFOMATION = "INFOMATION";
        private const string BACKGROUND_ITEM = "active";
        private Dictionary<string, string> navActiveClass = new Dictionary<string, string>(){
        {HOME, "" },
        {SHOP, "" },
        {CONTACT, "" },
        {INFOMATION,"" }};
        protected override async Task OnInitializedAsync()
        {
            string pageTitle = "";
            string urlName = navManager != null ?
                navManager.Uri.ToString().Replace(navManager.BaseUri, "/") : "";
            if (pageTitle == "")
            {
                navActiveClass[HOME] = BACKGROUND_ITEM;
            }
            if (pageTitle == "/shop")
            {
                navActiveClass[SHOP] = BACKGROUND_ITEM;
            }
            if (pageTitle == "/contact")
            {
                navActiveClass[CONTACT] = BACKGROUND_ITEM;
            }
            if (pageTitle == "/ShopInfo")
            {
                navActiveClass[INFOMATION] = BACKGROUND_ITEM;
            }
        }
        private void resetState()
        {
            navActiveClass = new Dictionary<string, string>(){
                {HOME, "" },
                {SHOP, "" },
                {CONTACT, "" },
                {INFOMATION,"" }
            };
        }
        private async void nav_click_handle(string action)
        {
            resetState();
            navActiveClass[action] = BACKGROUND_ITEM;
            StateHasChanged();
        }

        private async Task Logout()
        {
            var customAuthStateProvider = (Authentication)authStateProvider;
            await customAuthStateProvider.UpdateAuthentincationState(null);
            navManager.NavigateTo("/", true);
        }

        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : "bg-dark";

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
        private async void pass_data_to_cart()
        {
            var authState = await authenticationStateTask;
            string user = authState.User.Identity.Name;
            var getUser = await acs.GetCurrentCustomerByName(user);
            string customer_id = getUser.customer_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"customer_id", customer_id},
            };
            iredir.RedirectParameter("cart", passData);
        }
    }
}
