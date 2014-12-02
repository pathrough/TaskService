using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public class ADOBase<T> : IDALBase<T> where T : class,new()
    {
        #region ADO.net操作数据库的基本API
        //在这一部分需要提供各类操作数据的API,作用主要有两个:
        //第一，调用这些API实现IDALBase<T>接口
        //第二，对于所有继承自DALBase<T>的数据访问层实体，
        //他会取得该dalActive实体，让后强行转换为ADOBase<T>对象
        //调用此类API，实现其特有的功能
        #endregion

        #region 实现接口
        public virtual bool Insert(T entity)
        {
            //采用ADO实现
            return true;
        }

        public virtual bool Update(T entity)
        {
            //采用ADO实现
            return true;
        }

        public virtual bool Delete(T entity)
        {
            //采用ADO实现
            return true;
        }

        public virtual T Query(int ID)
        {
            //采用ADO实现
            return new T();
        }
        #endregion
    }
}
