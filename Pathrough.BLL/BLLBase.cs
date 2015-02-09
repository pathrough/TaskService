using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public abstract class BLLBase<T> where T : class
    {
        public BLLBase(IDALBase<T> dal)
        {
            this.dalService = dal;
        }
        /// <summary>
        /// 数据库接口
        /// </summary>
        protected IDALBase<T> dalService;

        public T GetEntity(long id)
        {
            return dalService.GetEntity(id);
        }       
    }
}
