using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using DATN.Model;

namespace DATN
{
    public class Authentication : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sesssionStorage;
        private ClaimsPrincipal _anomynous = new ClaimsPrincipal(new ClaimsIdentity());
        public Authentication(ProtectedSessionStorage sesssionStorage)
        {
            _sesssionStorage = sesssionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSesstionStorageResult = await _sesssionStorage.GetAsync<m_account>("m_account");
                var userSession = userSesstionStorageResult.Success ? userSesstionStorageResult.Value : null;
                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anomynous));
                var claimsprincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.username),
                new Claim(ClaimTypes.Role, userSession.role)
            }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(claimsprincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anomynous));
            } 
        }
        public async Task UpdateAuthentincationState(m_account userSesstion)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSesstion != null)
            {
                await _sesssionStorage.SetAsync("m_account", userSesstion);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSesstion.username),
                    new Claim(ClaimTypes.Role, userSesstion.role)
                }));
            }
            else
            {
                await _sesssionStorage.DeleteAsync("m_account");
                claimsPrincipal = _anomynous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
