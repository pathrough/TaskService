using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public class BidSourceConfigDAL : DALBase<BidSourceConfig>
    {
        public override void Insert(BidSourceConfig entity)
        {
            _Context.BidSourceConfigs.Add(entity);
            _Context.SaveChanges();
        }

        public List<BidSourceConfig> GetAll()
        {
            return  _Context.BidSourceConfigs.ToList();
        }
    }
}
