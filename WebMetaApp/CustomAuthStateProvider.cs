////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace WebMetaApp
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        readonly SessionMarkerLiteModel _session_marker;

        public CustomAuthStateProvider(SessionMarkerLiteModel set_session_marker)
        {
            _session_marker = set_session_marker;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            List<Claim> claims = new List<Claim>();
            if (!string.IsNullOrEmpty(_session_marker.Login))
            {
                claims.AddRange(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, _session_marker.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, _session_marker.AccessLevelUser.ToString())
                });

                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                ClaimsPrincipal id_claim = new ClaimsPrincipal(id);
                return Task.FromResult(new AuthenticationState(id_claim));
            }
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        public new void AuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
