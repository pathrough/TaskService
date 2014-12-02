using Pathrough.Common;
using Pathrough.Entity;
using Pathrough.Factory;
using Pathrough.IDAL;
using Pathrough.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public class BidBLL : BLLBase<Bid>
    {
        public BidBLL()
        {
            //初始化dal服务
            this.dalService = DALFactory.CreateOrder<IBidDAL>
                (FactoryConfig.Bid.AssemblyPath, FactoryConfig.Bid.ClassName);
        }
        public bool Inset(Bid bid)
        {
            //业务1
            //业务2
            return dalService.Insert(bid);
        }

        #region IOrderDAL的专用方法
        public void 专用方法Dom()
        {
            IBidDAL orderbll = dalService as IBidDAL;
            //调用IOrderDAL的专有方法
            //orderbll.专有方法();
        }
        #endregion
    }
}
