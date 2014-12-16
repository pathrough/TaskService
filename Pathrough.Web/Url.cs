using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Web
{
    public class Url
    {
        string url;
        public Url(string url)
        {
            this.url = url;
        }
        public string DomainUrl
        {
            get
            {
                try
                {
                    return url.Split(new string[] { "://" }, StringSplitOptions.None)[0] + "://" + new Uri(url).Host;
                }
                catch
                {
                    return "";
                } 
            }
        }

        public static string GetObsluteUrl(string currentUrl, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(currentUrl))
            {
                return relativeUrl;
            }
            else
            {
                string strAbsoluteUrl = relativeUrl;
                string homeUrl = new Url(currentUrl).DomainUrl;
                if (relativeUrl.Contains("://"))
                {
                    strAbsoluteUrl = relativeUrl;
                }
                else if (relativeUrl.StartsWith("/"))//根目录
                {
                    strAbsoluteUrl = homeUrl + relativeUrl;
                }
                else if (relativeUrl.StartsWith("./"))//显式当前目录
                {
                    int subStringIndex = currentUrl.EndsWith("/") ? 2 : 1;
                    strAbsoluteUrl = currentUrl + relativeUrl.Substring(subStringIndex);
                }
                else if (relativeUrl.StartsWith("../"))//上级或上几级目录
                {
                    string[] parts = relativeUrl.Split(new string[] { "../" }, StringSplitOptions.None);
                    int layerCount = parts.Length - 1;//后退多少级目录
                    string[] urlParts = currentUrl.Split(new char[] { '/' }, StringSplitOptions.None);
                    int reduceCount = currentUrl.EndsWith("/") ? 1 : 0;
                    strAbsoluteUrl = string.Join("/", urlParts, 0, urlParts.Length - reduceCount - layerCount) + relativeUrl.Substring(relativeUrl.LastIndexOf("../") + 2);
                }
                else//经典当前目录
                {
                    strAbsoluteUrl = homeUrl + "/" + relativeUrl;
                }
                if (strAbsoluteUrl.Length > 1000)
                {
                }
                return strAbsoluteUrl;
            }

        }
    }
}
