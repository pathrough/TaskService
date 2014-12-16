using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Web
{
    public class WebPageLoader
    {

        HttpWebUtility web = new HttpWebUtility();
        public HtmlDocument GetPage(string url)
        {
            HtmlDocument doc;
            Encoding encoding;
            GetHtml(url, out encoding, out doc);
            return doc;
        }
        string GetHtml(string url, out Encoding encoding, out HtmlDocument doc)
        {
            var ms = web.SimpleGetMemoryStream(url, "get");
            var e = Encoding.Default;
            string html = e.GetString(ms.ToArray());
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            try
            {
                string strEncoding = "";
                GetEncoding(doc.DocumentNode, ref strEncoding);
                encoding = e = Encoding.GetEncoding(strEncoding);
                html = e.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                encoding = e = Encoding.UTF8;
                html = e.GetString(ms.ToArray());
            }
            finally
            {
                doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                FixUrl(doc.DocumentNode, url);
                html = doc.DocumentNode.OuterHtml;
            }
            return html;
        }
        void FixUrl(HtmlNode node, string currentUrl)
        {
            string homeUrl = new Url(currentUrl).DomainUrl;
            if (node.Name == "a" || node.Name == "img")
            {
                foreach (var item in node.Attributes)
                {
                    if ((item.Name == "href" || item.Name == "src") && (item.Value.Contains("://") == false))//判断是否是完整的url，如http://www.baidu.com/djdj/
                    {
                        item.Value = Url.GetObsluteUrl(currentUrl, item.Value);
                    }
                }
            }
            foreach (var item in node.ChildNodes)
            {
                FixUrl(item, homeUrl);
            }
        }
        static void GetEncoding(HtmlNode node, ref string encoding)
        {
            if (node.Name == "meta")
            {
                foreach (var att in node.Attributes)
                {
                    if (att.Name == "content")
                    {
                        var parts = att.Value.Split(';');
                        foreach (var item in parts)
                        {
                            if (item.Trim().StartsWith("charset"))
                            {
                                var part2 = item.Split('=');
                                if (part2.Length == 2)
                                {
                                    encoding = part2[1];
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var child in node.ChildNodes)
                {
                    GetEncoding(child, ref encoding);
                }
            }
        }
    }
}
