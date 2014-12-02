using Pathrough.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Factory
{
    public class DALFactory
    {
        /// <summary>
        /// 创建指定类型T的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyPath">程序集路径</param>
        /// <param name="className">类名称(非全名)</param>
        /// <returns>T类型实例</returns>
        public static T CreateOrder<T>(string assemblyPath, string className)
        {
            className = string.Format("{0}.{1}", assemblyPath, className);
            object objType = DataCache.GetCache(className);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(assemblyPath).CreateInstance(className);
                    DataCache.SetCache(className, objType);// 写入缓存
                }
                catch (System.Exception ex)
                {
                    string str = ex.Message;// 记录错误日志
                }
            }
            return (T)objType;
        }
    }
}
