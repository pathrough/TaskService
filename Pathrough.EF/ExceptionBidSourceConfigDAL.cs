using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public class ExceptionBidSourceConfigDAL : DALBase<ExceptionBidSourceConfig>
    {
        public ExceptionBidSourceConfigDAL()
        {
        }
        public override void Insert(ExceptionBidSourceConfig entity)
        {
            _Context.ExceptionBidSourceConfigs.Add(entity);
            _Context.SaveChanges();
        }
    }
}
