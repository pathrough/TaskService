using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public class BLLBase<T> where T : class
    {
        /// <summary>
        /// 数据库接口
        /// </summary>
        protected IDALBase<T> dalService;

        public void Insert(T entity)
        {
            dalService.Insert(entity);
        }
    }
}
