using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public class DalActiveProvider<T> where T : class,new()
    {
        private static string className = ConfigurationManager.AppSettings["dalActiveName"];
        public static IDALBase<T> GetDalActive()
        {
            if (string.Equals(className, "EFBase"))
            {
                return new EFBase<T>();
            }
            else
            {
                return new ADOBase<T>();
            }
        }
    }
}
