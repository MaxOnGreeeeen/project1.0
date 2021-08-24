using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace projcet1._0.Models
{
    public class ArticleLike { 

        [Key]
        [Column(Order = 1)]
        public int LikeID { set; get; }
        public virtual Like Like { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ArticleID { set; get; }
        public virtual Article Article { get; set; }
        public int TotalLikeCount { set; get; }

    }
}
