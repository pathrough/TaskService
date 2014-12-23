using Pathrough.EF;
using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    interface IBidSourceConfigBLL
    {
        List<BidSourceConfig> GetAll();
        void Insert(BidSourceConfig entity);
    }
    public class BidSourceConfigBLL :BLLBase<BidSourceConfig>, IBidSourceConfigBLL
    {
        public BidSourceConfigBLL()
        {
            base.dalService = new BidSourceConfigDAL();
        }
        public List<BidSourceConfig> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
