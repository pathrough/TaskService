using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public class BidDAL : DALBase<Bid>, IBidDAL
    {
        public BidDAL()
        {
            this.dalActive = DalActiveProvider<Bid>.GetDalActive();
        }

        #region 专有公共方法
        //根据技术的选型
        //将dalActive强行转换
        //为ADOBase<T>或者EFBase,在调用其
        //数据访问API来实现专用公共方法
        #endregion
    }
}
