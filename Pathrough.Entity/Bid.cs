using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Entity
{
    public class Bid
    {
        public long? BidID { get; set; }
        public string BidTitle { get; set; }
        public string BidContent { get; set; }
        public string BidSourceUrl{get;set;}
    }
}
