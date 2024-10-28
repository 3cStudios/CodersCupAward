using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CodersCupAward.Models;
using Microsoft.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace CodersCupAward.Helper
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly IJSRuntime JsRuntime;

        public SecurityHelper(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        //public string GenerateJwtToken(ApplicationUser user, bool rememberMe)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, user.ApplicationUserUniqueId.ToString()),
        //            new Claim(ClaimTypes.Name, user.Username)
        //        }),
        //        Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        public async Task<string> GetRememberMeTokenAsync()
        {
            return await JsRuntime.InvokeAsync<string>("localStorage.getItem", "AuthToken");
        }
        public async Task SetRememberMeTokenAsync(ApplicationUser user)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.setItem", "AuthToken", user.ApplicationUserUniqueId.ToString());

        }

        public async Task RemoveRememberMeTokenAsync()
        {
            await JsRuntime.InvokeVoidAsync("localStorage.removeItem", "AuthToken");
        }


    }
}
