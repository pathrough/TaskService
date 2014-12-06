using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IDAL
{
    public interface IMaintainRecordInfo 
    {
    }

    public class Factory
    {
        /// <summary>
        /// 创建SqlMaintainRecordInfo数据层接口。维修日志表--韩义
        /// </summary>
        public static T CreateInstance<T>()
        {
            var t = typeof(T);
            object objType = CreateObject(t.Namespace, t.FullName);
            return (T)objType;
        }

        //使用缓存--韩义
        private static object CreateObject(string AssemblyPath, string classNamespace)
        {
            object objType = DataCache.GetCache(classNamespace);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);
                    DataCache.SetCache(classNamespace, objType);// 写入缓存
                }
                catch (System.Exception ex)
                {
                    string str = ex.Message;// 记录错误日志
                }
            }
            return objType;
        }
    }

    /// <summary>
    /// 缓存操作类--韩义
    /// </summary>
    public class DataCache
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值--韩义
        /// </summary>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值--韩义
        /// </summary>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }
    }
 
}
