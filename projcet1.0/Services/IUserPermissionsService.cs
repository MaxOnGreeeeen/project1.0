using System;
using projcet1._0.Models;

namespace projcet1._0.Services
{
    public interface IUserPermissionsService
    {
        bool CanEditArticle(Article article);
    }
}