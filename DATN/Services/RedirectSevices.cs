using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace DATN.Services
{
    public interface IRedirectSevices
    {
        void RedirectNormal(string router);
        void RedirectNormalForce(string router, bool IsForce);
        void RedirectParameter(string router, Dictionary<string, string> passData);
        void RedirectParameterForce(string router, Dictionary<string, string> passData, bool IsForce);
        System.Uri GetUri();
    }
    public class RedirectSevices : IRedirectSevices
    {
        private readonly NavigationManager _NaviManager;

        public RedirectSevices(NavigationManager NaviManager)
        {
            _NaviManager = NaviManager;
        }
         public Uri GetUri()
        {
            return _NaviManager.ToAbsoluteUri(_NaviManager.Uri);
        }

        public void RedirectNormal(string router)
        {
            _NaviManager.NavigateTo(router);
        }

        public void RedirectNormalForce(string router, bool IsForce)
        {
            _NaviManager.NavigateTo(router, IsForce);
        }

        public void RedirectParameter(string router, Dictionary<string, string> passData)
        {
            _NaviManager.NavigateTo(QueryHelpers.AddQueryString(router, passData));
        }

        public void RedirectParameterForce(string router, Dictionary<string, string> passData, bool IsForce)
        {
            _NaviManager.NavigateTo(QueryHelpers.AddQueryString(router, passData), IsForce);
        }
    }
}
