using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectCacheDemo
{
    class MyClass
    {
        /// <summary>
        /// 创建SqlMaintainRecordInfo数据层接口。维修日志表--韩义
        /// </summary>
        public static IDAL.IMaintainRecordInfo CreateT_MaintainRecordInfo()
        {

            string ClassNamespace = AssemblyPath + ".SqlMaintainRecordInfo";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (IDAL.IMaintainRecordInfo)objType;
        }
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
   
}
