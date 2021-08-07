using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models.ViewModels
{
    public class CategoryCreateModel
    {
        [Required]
        public string CategoryOfArticle { set; get; }
    }
}
