using Pathrough.EF;
using Pathrough.Entity;
using Pathrough.IDAL;
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
            : base(new ExceptionBidSourceConfigDAL())
        {
        }
        public void Insert(ExceptionBidSourceConfig entity)
        {
            dalService.Insert(entity);
        }
    }
}
