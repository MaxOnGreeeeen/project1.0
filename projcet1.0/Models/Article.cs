using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models
{
    public class Article
    {
        public int Id { set; get; }
        [MaxLength(25)]
        public string Name { set; get;}

        public string Author { set; get; }

        public string Location { set; get; }

        [MaxLength(10000)]
        public string Text { set; get; }

        public string Img { set; get; }

        [Required]
        public int CategoryId { set; get; }

        public Category Categories { set; get; }

        public DateTime Data { set; get; }

    }
}
