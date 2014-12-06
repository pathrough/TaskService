using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Factory
{
    public class BidDALFactory
    {

        private static readonly string AssemblyName = ConfigurationManager.AppSettings["Assembly"];
        private static readonly string className = ConfigurationManager.AppSettings["className"];
        public static IBidDAL CreateInstance()
        {
            return (IBidDAL)Assembly.Load(AssemblyName).CreateInstance(className);
        }
    }
}
