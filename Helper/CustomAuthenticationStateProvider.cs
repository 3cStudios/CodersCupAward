using System.Security.Claims;
using CodersCupAward.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace CodersCupAward.Helper
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
    
        private ApplicationUser _currentUser;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity claimsIdentity;

            if (_currentUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, _currentUser.EmailAddress)
                };
                var userRoles = _currentUser
                    .ApplicationUserRole
                    .Select(role => new Claim(ClaimTypes.Role, role.ApplicationRoles.Name))
                    .ToList();
                claims.AddRange(userRoles);
                claimsIdentity = new ClaimsIdentity(claims, "apiauth_type");
                
            }
            else
            {
                claimsIdentity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(claimsIdentity);
            return Task.FromResult(new AuthenticationState(user));

        }

        public void MarkUserAsAuthenticated(ApplicationUser user)
        {
            _currentUser = user;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        }

        public void MarkUserAsLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

      
    }

}
