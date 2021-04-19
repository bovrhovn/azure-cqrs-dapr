using System.Collections.Generic;
using System.Security.Claims;
using CityApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CityApp.Web.Helpers
{
    public static class IdentityHelper
    {
        public static ClaimsPrincipal GenerateClaims(this CityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.CityUserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}