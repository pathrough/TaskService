using HtmlAgilityPack;
using Pathrough.LuceneSE;
using Pathrough.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pathrough.BLL.Spider
{
    public class BidWebsiteSpider
    {
        Queue<string> _ListUrlQueue = new Queue<string>();
        public void LoadListUrlQueue(string startListUrl, string[] listPattern)
        {
            List<string> temUrlList = new List<string>() { startListUrl };
            string domain = new Url(startListUrl).DomainUrl;
            Stack<List<string>> urlListStack = new Stack<List<string>>();
            List<string> distinctUrlList = new List<string>();
            urlListStack.Push(temUrlList);
            do
            {
                var list = urlListStack.Pop();
                foreach (var urlitem in list)
                {
                    GetPage(domain, urlListStack, urlitem, ref distinctUrlList, listPattern);
                }
            }
            while (urlListStack.Count > 0);
            listOver = true;
        }

        bool listOver = false;

        public void DownLoadDetail(string[] detailPattern)
        {
            while (listOver==false)
            {
                if (_ListUrlQueue.Count > 0)
                {
                    var url = _ListUrlQueue.Dequeue();
                    //Console.WriteLine("-" + url);
                    string domain = new Url(url).DomainUrl;
                    var list = GetDetailUrlListByUrl(url, detailPattern, domain);
                    foreach (var item in list)
                    {
                        Console.WriteLine(item);
                        HtmlDocument doc = new WebPageLoader().GetPage(item);
                        var text = doc.DocumentNode.InnerText;
                        if(!string.IsNullOrWhiteSpace(text))
                        {
                            BidSearchEngine.Current.CreateIndex(new List<Entity.Bid> { new Entity.Bid { BidContent = text, BidID = 1, BidSourceUrl = item,BidTitle="" } });
                        }                        
                    }
                } 
                else
                {
                    Thread.Sleep(100);
                }
            };                              
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
        private void GetPage(string domain, Stack<List<string>> urlListStack, string urlitem
            , ref List<string> distinctUrlList
            , string[] listPattern
            )
        {
            bool matchList = IsMatchOne(urlitem, listPattern);

            if (string.IsNullOrWhiteSpace(urlitem) == false
                && distinctUrlList.Contains(urlitem) == false
                && urlitem.StartsWith(domain)
                && matchList
                )
            {
                //Console.WriteLine(urlitem);
                distinctUrlList.Add(urlitem);
                _ListUrlQueue.Enqueue(urlitem);
                //Console.WriteLine("+" + urlitem);


                string html = GetWebPageContent(urlitem);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                //列表页，url分析
                List<string> urlList = new List<string>();

                Stack<HtmlNode> stack = new Stack<HtmlNode>();
                stack.Push(doc.DocumentNode);
                do
                {
                    var curNode = stack.Pop();
                    AddUrl(distinctUrlList, urlitem, urlList, curNode, domain, listPattern);
                    if (curNode.HasChildNodes)
                    {
                        foreach (var node in curNode.ChildNodes)
                        {
                            stack.Push(node);
                            AddUrl(distinctUrlList, urlitem, urlList, node, domain, listPattern);
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

        private List<string> GetUrlListByUrl(string urlitem,Func<string, string> GetCompleteUrl, Func<string, bool> UrlIsMatch)
        {
            string html = GetWebPageContent(urlitem);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            //列表页，url分析
            List<string> urlList = new List<string>();

            Stack<HtmlNode> stack = new Stack<HtmlNode>();
            stack.Push(doc.DocumentNode);
            do
            {
                var curNode = stack.Pop();
                AddUrl(curNode, urlList, GetCompleteUrl,(d)=>{
                    return UrlIsMatch(d) && urlList.Contains(d) == false;
                });
                if (curNode.HasChildNodes)
                {
                    foreach (var node in curNode.ChildNodes)
                    {
                        stack.Push(node);
                        AddUrl(node, urlList, GetCompleteUrl, UrlIsMatch);
                    }
                }
            }
            while (stack.Count > 0);
            return urlList;
        }

        private List<string> GetDetailUrlListByUrl(string urlitem, string[] detailPattern, string domain)
        {
            return GetUrlListByUrl(urlitem, (relativeUrl) =>
            {
                return Url.GetObsluteUrl(urlitem, relativeUrl);
            }, (d) =>
            {
                return string.IsNullOrWhiteSpace(d) == false && d.StartsWith(domain)
                    && IsMatchOne(d, detailPattern);
            });
        }

        private static void AddUrl(List<string> distinctUrlList, string curUrl, List<string> urlList, HtmlNode node, string domain, string[] listPattern)
        {
            AddUrl(node, urlList,
                (relativeUrl) =>
                {
                    return Url.GetObsluteUrl(curUrl, relativeUrl);
                },
                (d) => {
                return string.IsNullOrWhiteSpace(d) == false && distinctUrlList.Contains(d) == false && d.StartsWith(domain)
                    && IsMatchOne(d, listPattern);
                });            
        }

        private static string AddUrl(HtmlNode node, List<string> urlList, Func<string, string> ProcessUrl, Func<string, bool> IsPass)
        {
            if (node.Name == "a")
            {
                if (node.HasAttributes)
                {
                    foreach (var attr in node.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            string url = ProcessUrl(attr.Value);
                            if (IsPass(url))
                            {
                                urlList.Add(url);
                            }
                        }
                    }
                }
            }
            return null;
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
