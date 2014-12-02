using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public class EFBase<T> : IDALBase<T> where T : class,new()
    {
        #region  EF操作数据库的API
        //这里面需要提供EF操作数据库的各类方法
        //函数
        #endregion

        #region 调用EF操作数据库的API实现接口
        public virtual bool Insert(T entity)
        {
            //采用EF
            return true;
        }

        public virtual bool Update(T entity)
        {
            //采用EF
            return true;
        }

        public virtual bool Delete(T entity)
        {
            //采用EF
            return true;
        }

        public virtual T Query(int ID)
        {
            //采用EF
            return new T();
        }
        #endregion
    }
}
