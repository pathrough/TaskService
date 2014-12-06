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
    public class BidBLL
    {
        protected IBidDAL orderDal;
        public BidBLL()
        {
            //在这里需要实力化orderDal
            orderDal = BidDALFactory.CreateInstance();
        }

        public bool Insert(BidModel model)
        {
            //业务点1
            //业务点2
            return orderDal.Insert();
        }

        /// <summary>
        /// 修改Order表数据
        /// </summary>
        /// <param name="model">表数据对应实体</param>
        /// <returns>成功:true,失败:false</returns>
        public bool Update(BidModel model)
        {
            //业务点1
            //业务点2
            return orderDal.Update(model);
        }
        /// <summary>
        /// 删除Order表指定ID的记录
        /// </summary>
        /// <param name="id">表ID</param>
        /// <returns>成功:true,失败:false</returns>
        public bool Delete(int id)
        {
            //业务点1
            //业务点2
            return orderDal.Delete(id);
        }
    }
}
