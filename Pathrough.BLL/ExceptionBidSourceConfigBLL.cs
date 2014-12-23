using Pathrough.EF;
using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public class ExceptionBidSourceConfigBLL : BLLBase<ExceptionBidSourceConfig>
    {
        public ExceptionBidSourceConfigBLL()
        {
            base.dalService = new ExceptionBidSourceConfigDAL();
        }
        public void Insert(ExceptionBidSourceConfig entity)
        {
            base.dalService.Insert(entity);
        }
    }
}
