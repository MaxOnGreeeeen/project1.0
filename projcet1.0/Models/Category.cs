using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models
{
    public class Category
    {
        public int Id { set; get; }

        public string CategoryOfArticle{ set; get; }

        public int Count { set; get; }
        public List<Article> Articles { set; get; }
    }
}
