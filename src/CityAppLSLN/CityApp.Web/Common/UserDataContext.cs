using System.Security.Claims;
using CityApp.Web.Models;
using Microsoft.AspNetCore.Http;

namespace CityApp.Web.Common
{
    public class UserDataContext : IUserDataContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserDataContext(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        public CityUserViewModel GetCurrentUser()
        {
            var httpContextUser = httpContextAccessor.HttpContext.User;
            if (httpContextUser == null) return null;

            var currentUser = new CityUserViewModel();

            var claimName = httpContextUser.FindFirst(ClaimTypes.Name);
            currentUser.Fullname = claimName.Value;

            var claimId = httpContextUser.FindFirst(ClaimTypes.NameIdentifier);
            currentUser.CityUserId = int.Parse(claimId.Value);

            var claimEmail = httpContextUser.FindFirst(ClaimTypes.Email);
            currentUser.Email = claimEmail.Value;

            return currentUser;
        }
    }
}