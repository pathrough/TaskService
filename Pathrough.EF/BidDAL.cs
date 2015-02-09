using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public class BidDAL : DALBase<Bid>,IBidDAL 
    {
        public BidDAL()
        {
        }
        //public override void Insert(Bid entity)
        //{
        //    _Context.Bids.Add(entity);
        //    _Context.SaveChanges();
        //}

        //public override void Update(Bid entity)
        //{
        //    _Context.SaveChanges();
        //}

        public Bid GetEntityByUrl(string url)
        {
            return _Context.Bids.FirstOrDefault(d=>d.BidSourceUrl==url);
        }


        public List<Bid> GetPageList(int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            var query = _Context.Bids.OrderByDescending(d => d.BidID);
            return GetPageList(query, pageIndex, pageSize, out  pageCount, out  recordCount);
        }
    }
}
