using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.IDAL
{
    public interface IDALBase<T> where T : class
    {
        /// <summary>
        /// 向T对应的数据表插入
        /// 一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true:成功,false:失败</returns>
        void Insert(T entity);
        /// <summary>
        /// 修改数据表中与entity
        /// 实体对应的记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true:成功,false:失败</returns>
        void Update(T entity);
        /// <summary>
        /// 删除数据表中与entity
        /// 实体对应的记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true:成功,false:失败</returns>
        void Delete(T entity);
        /// <summary>
        /// 查询数据库表中指定的ID
        /// 的记录
        /// </summary>
        /// <param name="ID">标识ID</param>
        /// <returns>指定ID记录对应的实体</returns>
        T GetEntity(long ID);       
    }
}
