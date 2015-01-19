using HtmlAgilityPack;
using Pathrough.BLL;
using Pathrough.BLL.Spider;
using Pathrough.Entity;
using Pathrough.LuceneSE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LuceneConsoleApplication1
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //var config = new BidSourceConfig
            //{
            //    ListUrl = "http://www.chinabidding.com/zbzx.jhtml?method=outlineOne&type=biddingProjectGG&channelId=205"
            //    ,
            //    DetailUrlPattern = @"http://www.chinabidding.com/zbzx-detail-\d+.html"
            //    ,
            //    TitleXpath = "/html/body/div/div[2]/div[2]/div[1]/div/h2"
            //    ,
            //    ContentXpath = "/html/body/div/div[2]/div[2]/div[1]/div/div[2]"
            //    ,
            //    PubishDateXpath = "/html/body/div/div[2]/div[2]/div[1]/div/div[1]"
            //    ,
            //    PubishDatePattern = @"(\d{4}\.\d{2}\.\d{2})"
            //};

            BidSourceConfigBLL configService = new BidSourceConfigBLL();
            var configList = configService.GetAll();

            //configService.Insert(config);
            BidWebsiteSpider sp = new BidWebsiteSpider();
            foreach (var config in configList)
            {
                if(config!=null && !string.IsNullOrWhiteSpace(config.ListUrl))
                {
                    var bidList = sp.DownLoadBids(config);

                    BidBLL bidService = new BidBLL();
                    foreach (var entity in bidList)
                    {
                        bidService.Insert(entity);
                    }

                    bidService.CreateLuceneIndex(bidList);
                }
              
            }
            

            Console.ReadKey();
        }

        public static void DownloadWebsite(string url, string[] listPattern, string[] detailPattern)
        {

            List<string> urlList1 = new List<string>() { url };
            string domain = new UrlHelper().GetHomeUrl(url);
            Stack<List<string>> urlListStack = new Stack<List<string>>();
            List<string> distinctUrlList = new List<string>();
            urlListStack.Push(urlList1);
            do
            {
                var list = urlListStack.Pop();
                foreach (var urlitem in list)
                {
                    GetPage(domain, urlListStack, urlitem, ref distinctUrlList, listPattern, detailPattern);
                }
            }
            while (urlListStack.Count > 0);
        }
        static bool IsMatchOne(string input, string[] patternList)
        {
            bool match = false;
            foreach (var pattern in patternList)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
                {
                    match = true;
                    break;
                }
            }
            return match;
        }
        private static void GetPage(string domain, Stack<List<string>> urlListStack, string urlitem
            , ref List<string> distinctUrlList
            , string[] listPattern, string[] detailPattern
            )
        {
            bool matchList = IsMatchOne(urlitem, listPattern);
            bool matchDetail = IsMatchOne(urlitem, detailPattern);

            if (string.IsNullOrWhiteSpace(urlitem) == false 
                && distinctUrlList.Contains(urlitem) == false 
                && urlitem.StartsWith(domain)
                && (matchList || matchDetail)
                )
            {
                Console.WriteLine(urlitem);
                distinctUrlList.Add(urlitem);
                string html = GetWebPageContent(urlitem);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (matchDetail)
                {
                    //底层页
                }
                else if (matchList)
                {
                    //列表页，url分析
                    List<string> urlList = new List<string>();

                    Stack<HtmlNode> stack = new Stack<HtmlNode>();
                    stack.Push(doc.DocumentNode);
                    do
                    {
                        var curNode = stack.Pop();
                        AddUrl(distinctUrlList, urlitem, urlList, curNode, domain, listPattern, detailPattern);
                        if (curNode.HasChildNodes)
                        {
                            foreach (var node in curNode.ChildNodes)
                            {
                                stack.Push(node);
                                AddUrl(distinctUrlList, urlitem, urlList, node, domain, listPattern, detailPattern);
                            }
                        }
                    }
                    while (stack.Count > 0);
                    if (urlList.Count > 0)
                    {
                        urlListStack.Push(urlList);
                    }
                }
            }
        }
        private static void AddUrl(List<string> distinctUrlList, string curUrl, List<string> urlList, HtmlNode node, string domain, string[] listPattern, string[] detailPattern)
        {
            if (node.Name == "a")
            {
                if (node.HasAttributes)
                {
                    foreach (var attr in node.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            string url = new UrlHelper().GetObsluteUrl(curUrl, attr.Value);
                            bool matchList = IsMatchOne(url, listPattern);
                            bool matchDetail = IsMatchOne(url, detailPattern);

                            if (string.IsNullOrWhiteSpace(url) == false && distinctUrlList.Contains(url) == false && url.StartsWith(domain)
                                && (matchList || matchDetail)
                                )
                            {
                                urlList.Add(url);
                            }
                        }
                    }
                }
            }
        }

        private static string GetWebPageContent(string strUrl)
        {
            string strMsg = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                strMsg = reader.ReadToEnd();

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }
            return strMsg;
        }
    }
}

public class UrlHelper
{
    public string GetObsluteUrl(string currentUrl, string relativeUrl)
    {
        string strCurrentDir = GetCurrentDir(currentUrl).Trim('/');
        string homeUrl = GetHomeUrl(currentUrl);
        if (currentUrl.Contains("://"))//currentUrl必须是绝对完整的路径 
        {
            string strAbsoluteUrl = relativeUrl;
            if (relativeUrl.Contains("://"))//已经是完成的url了，如http://www.tgnet.com 
            {
                strAbsoluteUrl = relativeUrl;
            }
            else if (relativeUrl.StartsWith("/"))//根目录，如/project/details.html 
            {

                strAbsoluteUrl = homeUrl + relativeUrl;
            }
            else if (relativeUrl.StartsWith("./"))//显式当前目录，如 ./details.html 
            {
                strAbsoluteUrl = strCurrentDir + "/" + relativeUrl.Substring(2);
            }
            else if (relativeUrl.StartsWith("../"))//上级或上几级目录， 如../../details.html 
            {
                string effectivePart = relativeUrl.Substring(relativeUrl.LastIndexOf("../") + 3);//获取../后面的东西,如../1.htm变成1.html 
                if (strCurrentDir != homeUrl)
                {
                    return strCurrentDir.Substring(0, strCurrentDir.LastIndexOf('/')) + "/" + effectivePart;
                }
                else
                {
                    //错误 
                    return homeUrl + "/" + effectivePart;
                }
                string[] parts = relativeUrl.Split(new string[] { "../" }, StringSplitOptions.None);
                int layerCount = parts.Length - 1;//后退多少级目录 

                string[] urlParts = currentUrl.Split(new char[] { '/' }, StringSplitOptions.None);
                int reduceCount = currentUrl.EndsWith("/") ? 1 : 0;
                strAbsoluteUrl = string.Join("/", urlParts, 0, urlParts.Length - reduceCount - layerCount) + relativeUrl.Substring(relativeUrl.LastIndexOf("../") + 2);
            }
            else//经典当前目录，如project/details.html 
            {
                strAbsoluteUrl = GetCurrentDir(currentUrl).Trim('/') + "/" + relativeUrl;
            }
            return strAbsoluteUrl;
        }
        else
        {
            return relativeUrl;
        }
    }

    public string GetCurrentDir(string url)
    {
        url = url.Trim('/');
        if (url.Count(d => d == '/') > 2)//只有更目录如http://www.tgnet.com 
        {
            return url.Substring(0, url.LastIndexOf('/'));
        }
        else
        {
            return url;
        }

    }

    /// <summary> 
    /// 根据一个url，获取http://www.tgnet.com形式的url 
    /// </summary> 
    /// <param name="url"></param> 
    /// <returns></returns> 
    public string GetHomeUrl(string url)
    {
        string homeUrl = "";
        if (!string.IsNullOrWhiteSpace(url))
        {
            if (url.Contains("://"))
            {
                string[] urlParts = url.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
                homeUrl = urlParts[0] + "://" + new Uri(url).Host;
            }
            else
            {
                homeUrl = "";//"http://" + url.Substring(0,url.IndexOf('/')); 
            }

        }
        return homeUrl;
    }

}