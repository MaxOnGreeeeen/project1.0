using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models
{

    public class Like
    {
        public int Id { set; get; }

        public DateTime DateTime { get; set; }

        public ICollection<ArticleLike> Articles { get; set; }
    }
}
