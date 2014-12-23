using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public class BidDAL : DALBase<Bid>,IBidDAL  //: DALBase<Bid>, IBidDAL
    {
        public BidDAL()
        {
            _Context = new BidContext();
        }
        public override void Insert(Bid entity)
        {
            _Context.Bids.Add(entity);
            _Context.SaveChanges();
        }

        public override void Update(Bid entity)
        {
            _Context.SaveChanges();
        }
    }
}
