using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pathrough.Entity
{
    public class Bid
    {
        [Key]
        public long BidID { get; set; }
        public string BidTitle { get; set; }
        public string BidContent { get; set; }
        public string BidSourceUrl{get;set;}

        public DateTime? BidPublishDate { get; set; }
        public DateTime? CreateDate { get; set; }

        public bool? WasIndexed { get; set; }

        public static Bid GetDefaultEntity()
        {
            return new Bid {CreateDate=DateTime.Now,WasIndexed=false };
        }

    }
}
