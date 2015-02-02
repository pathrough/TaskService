using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public class BidContext:DbContext
    {
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidSourceConfig> BidSourceConfigs { get; set; }
        public DbSet<ExceptionBidSourceConfig> ExceptionBidSourceConfigs { get; set; }
        public DbSet<Area> Areas { get; set; }
        public BidContext():base("name=BidContext")
        {

        }
    }
}
