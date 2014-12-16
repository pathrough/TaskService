using HtmlAgilityPack;
using Pathrough.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidSpiderConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //// string url = "http://www.bjztb.gov.cn/";
           string url = "http://www.ifeng.com/";

            //List<string> hasVisitList = new List<string>();
            //Run(url, ref hasVisitList, "/html[1]/body[1]/center[1]/table[3]/tr[1]/td[2]/table[5]");

            Url urlHelper = new Url(url);
            var domainUrl = urlHelper.DomainUrl;
            var sp = new WebPageLoader();
            var doc = sp.GetPage(url);
            var urlList = new HtmlParser(doc.DocumentNode.InnerHtml).GetUrlList(url);
            var visitedUrls = new List<string>();
            visitedUrls.Add(url);
            Console.WriteLine(url);
            int count = 0;
            do{
                for (int i = 0; i < urlList.Count; i++)
                {
                    if (!visitedUrls.Contains(urlList[i]))
                    {
                        var fixUrl = Url.GetObsluteUrl(url, urlList[i]);
                        if(fixUrl.StartsWith(domainUrl))
                        {
                            var doc1 = sp.GetPage(fixUrl);
                            urlList.AddRange(new HtmlParser(doc1.DocumentNode.InnerHtml).GetUrlList(urlList[i]));
                        }
                        visitedUrls.Add(urlList[i]);
                        try
                        {
                            urlList.Remove(urlList[i]);
                            Console.WriteLine(count + "---" + urlList[i]);
                        }
                        catch 
                        {
                        }
                        
                        count++;
                    }
                    else
                    {
                        try
                        {
                            urlList.Remove(urlList[i]);
                        }
                        catch 
                        {
                            
                        }
                    }
                }
            }
            while (urlList.Count>0);
            
            Console.WriteLine("完毕！");
            Console.ReadKey();

        }
        public static HtmlNode FindNode(HtmlNode thisNode, string target)
        {
            if (thisNode.InnerText.Contains(target))
            {
                if (thisNode.HasChildNodes)
                {
                    foreach (var node in thisNode.ChildNodes)
                    {
                        if (node.InnerText.Contains(target))
                        {
                            return FindNode(node, target);
                        }
                    }
                }
                else
                {
                    return thisNode;
                }
            }
            return null;
        }

        public static void Run(string url, ref  List<string> hasVisitList, string xpath)
        {
            Url urlHelper = new Url(url);

            //string url = "http://www.cnblogs.com/top5/archive/2011/03/08/1976917.html";
            var sp = new WebPageLoader();
            Encoding encoding;
            HtmlDocument doc = sp.GetPage(url);
            string html = doc.DocumentNode.InnerHtml;
            if (doc != null)
            {
                var node = doc.DocumentNode.SelectSingleNode(xpath);
                //if (node != null)
                //{
                //    File.WriteAllText("files/" + Guid.NewGuid() + ".html", node.InnerText);
                Console.WriteLine(hasVisitList.Count + "   " + url.Substring(0, url.Length > 100 ? 100 : url.Length));
                //}

            }

            hasVisitList.Add(url);

            List<string> urlList = new HtmlParser(html).GetUrlList(url);
            foreach (var itemUrl in urlList)
            {
                //如果是当前站点并且没有爬过
                if (hasVisitList.Contains(Url.GetObsluteUrl(url,itemUrl)) == false && itemUrl.StartsWith(urlHelper.DomainUrl))
                {
                    Run(itemUrl, ref hasVisitList, xpath);
                }
            }
        }
    }
}
