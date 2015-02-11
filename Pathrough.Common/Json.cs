using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Pathrough.Common
{
    public static class Json
    {
        public static string ToJson(object o)
        {
            JavaScriptSerializer servializer = new JavaScriptSerializer();
            return servializer.Serialize(o);
        }

        public static string ToJson<T>(this JsonResultEntity<T> entity)
        {
            JavaScriptSerializer servializer = new JavaScriptSerializer();
            return servializer.Serialize(entity);
        }

        public static string ToJson(this JsonResultEntity entity)
        {
            JavaScriptSerializer servializer = new JavaScriptSerializer();
            return servializer.Serialize(entity);
        }
    }

    public class JsonResultEntity
    {
        public string Msg { get; set; }
        public int Result { get; set; }

    }

    public class JsonResultEntity<T> : JsonResultEntity
    {
        public T Data { get; set; }
    }
}
