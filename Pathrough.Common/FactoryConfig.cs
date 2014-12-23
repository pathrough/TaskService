using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Common
{
    public class FactoryConfig
    {
        public static ConfigEntity Bid
        {
            get
            {
                return new ConfigEntity()
                {
                    //AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"],
                    //ClassName = ConfigurationManager.AppSettings["ClassName"]
                    AssemblyPath = "Pathrough.EF",
                    ClassName = "BidDAL"
                };
            }
        }
    }
}
