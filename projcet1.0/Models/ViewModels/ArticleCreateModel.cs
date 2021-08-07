using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models.ViewModels
{
    public class ArticleCreateModel
    {
        public string Name { set; get; } 

        public string Location { set; get; }

        [MaxLength(10000)]
        public string Text { set; get; }

        public IFormFile File { get; set; }

        [Required]
        public int CategoryId { set; get; }

    }
}
