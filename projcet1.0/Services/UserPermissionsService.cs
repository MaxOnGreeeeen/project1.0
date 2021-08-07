using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projcet1._0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using project1._0.Models;

namespace projcet1._0.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPermissionsService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool CanEditArticle(Article article)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == article.Author;
        }

        private HttpContext HttpContext => this.httpContextAccessor.HttpContext;

    }
}
