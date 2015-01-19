using HtmlAgilityPack;
using Pathrough.Entity;
using Pathrough.LuceneSE;
using Pathrough.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pathrough.BLL.Spider
{
    public class BidWebsiteSpider
    {
        public List<Bid> DownLoadBids(BidSourceConfig config)
        {
            var exampleDoc = new WebPageLoader().GetPage("http://www.chinabidding.com/zbzx-detail-224318166.html");
          
            string listUrl = config.ListUrl;
            List<Bid> bidList = new List<Bid>();
            string domain = new Url(listUrl).DomainUrl;
            var list = GetDetailUrlListByUrl(listUrl, config.DetailUrlPattern.Split('|'), domain);
            foreach (var item in list)
            {
                Console.WriteLine(item);
                HtmlDocument doc = new WebPageLoader().GetPage(item);
                var text = doc.DocumentNode.InnerText;
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var bid = Bid.GetDefaultEntity();
                    bid.BidSourceUrl = item;
                    try
                    {
                        bid.BidTitle = doc.DocumentNode.SelectSingleNode(config.TitleXpath).InnerText;
                    }
                    catch (Exception e)
                    {
                        ExceptionBidSourceConfigBLL ebsc = new ExceptionBidSourceConfigBLL();
                        ebsc.Insert(new ExceptionBidSourceConfig { Config = config, Msg = "BidTitle，根据xPath获取时失败！" });
                        throw e;
                    }
                    string strPubTime = "";
                    try
                    {
                        strPubTime = doc.DocumentNode.SelectSingleNode(config.PubishDateXpath).InnerText;
                    }
                    catch (Exception e)
                    {
                        //todo:记录获取失败
                        ExceptionBidSourceConfigBLL ebsc = new ExceptionBidSourceConfigBLL();
                        ebsc.Insert(new ExceptionBidSourceConfig { Config = config, Msg = "BidPublishDate，根据xPath获取时失败！" });
                        throw e;
                    }
                    var m = Regex.Match(strPubTime, config.PubishDatePattern);
                    if (m.Success)
                    {
                        bid.BidPublishDate = GetDateTime(m.Groups[1].Value);
                    }
                    try
                    {
                        bid.BidContent = doc.DocumentNode.SelectSingleNode(config.ContentXpath).InnerText;
                    }
                    catch (Exception e)
                    {
                        //todo:记录获取失败
                        ExceptionBidSourceConfigBLL ebsc = new ExceptionBidSourceConfigBLL();
                        ebsc.Insert(new ExceptionBidSourceConfig { Config = config, Msg = "BidContent，根据xPath获取时失败！" });
                        throw e;
                    }
                    
                    bidList.Add(bid);
                }
            }
            return bidList;
        }

        public DateTime? GetDateTime(string input)
        {
            DateTime? result = null;
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (Regex.IsMatch(input, ""))
                {
                    DateTime dt;
                    if (DateTime.TryParse(input, out dt))
                    {
                        return dt;
                    }
                }
            }
            return result;
        }

        public string GetXpath(string content, HtmlDocument doc)
        {
            Stack<HtmlNode> stack = new Stack<HtmlNode>();
            stack.Push(doc.DocumentNode);
            while (stack.Count > 0)
            {
                var curNode = stack.Pop();
                if (curNode.OuterHtml == content)
                {
                    return curNode.XPath;
                }
                else
                {
                    foreach (var node in curNode.ChildNodes)
                    {
                        if (node.OuterHtml == content)
                        {
                            return node.XPath;
                        }
                        else if (node.OuterHtml.Contains(content))
                        {
                            stack.Push(node);
                        }
                    }
                }
            };
            return "";
        }

        //Queue<string> _ListUrlQueue = new Queue<string>();
        //public void LoadListUrlQueue(string startListUrl, string[] listPattern)
        //{
        //    List<string> temUrlList = new List<string>() { startListUrl };
        //    string domain = new Url(startListUrl).DomainUrl;
        //    Stack<List<string>> urlListStack = new Stack<List<string>>();
        //    List<string> distinctUrlList = new List<string>();
        //    urlListStack.Push(temUrlList);
        //    do
        //    {
        //        var list = urlListStack.Pop();
        //        foreach (var urlitem in list)
        //        {
        //            GetPage(domain, urlListStack, urlitem, ref distinctUrlList, listPattern);
        //        }
        //    }
        //    while (urlListStack.Count > 0);
        //    listOver = true;
        //}

        //bool listOver = false;

        //public void DownLoadDetail(string[] detailPattern)
        //{
        //    while (listOver==false)
        //    {
        //        if (_ListUrlQueue.Count > 0)
        //        {
        //            var url = _ListUrlQueue.Dequeue();
        //            //Console.WriteLine("-" + url);
        //            string domain = new Url(url).DomainUrl;
        //            var list = GetDetailUrlListByUrl(url, detailPattern, domain);
        //            foreach (var item in list)
        //            {
        //                Console.WriteLine(item);
        //                HtmlDocument doc = new WebPageLoader().GetPage(item);
        //                var text = doc.DocumentNode.InnerText;
        //                if(!string.IsNullOrWhiteSpace(text))
        //                {
        //                    BidSearchEngine.Current.CreateIndex(new List<Entity.Bid> { new Entity.Bid { BidContent = text, BidID = 1, BidSourceUrl = item,BidTitle="" } });
        //                }                        
        //            }
        //        } 
        //        else
        //        {
        //            Thread.Sleep(100);
        //        }
        //    };                              
        //}
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
        //private void GetPage(string domain, Stack<List<string>> urlListStack, string urlitem
        //    , ref List<string> distinctUrlList
        //    , string[] listPattern
        //    )
        //{
        //    bool matchList = IsMatchOne(urlitem, listPattern);

        //    if (string.IsNullOrWhiteSpace(urlitem) == false
        //        && distinctUrlList.Contains(urlitem) == false
        //        && urlitem.StartsWith(domain)
        //        && matchList
        //        )
        //    {
        //        //Console.WriteLine(urlitem);
        //        distinctUrlList.Add(urlitem);
        //        _ListUrlQueue.Enqueue(urlitem);
        //        //Console.WriteLine("+" + urlitem);


        //        string html = GetWebPageContent(urlitem);
        //        HtmlDocument doc = new HtmlDocument();
        //        doc.LoadHtml(html);

        //        //列表页，url分析
        //        List<string> urlList = new List<string>();

        //        Stack<HtmlNode> stack = new Stack<HtmlNode>();
        //        stack.Push(doc.DocumentNode);
        //        do
        //        {
        //            var curNode = stack.Pop();
        //            AddUrl(distinctUrlList, urlitem, urlList, curNode, domain, listPattern);
        //            if (curNode.HasChildNodes)
        //            {
        //                foreach (var node in curNode.ChildNodes)
        //                {
        //                    stack.Push(node);
        //                    AddUrl(distinctUrlList, urlitem, urlList, node, domain, listPattern);
        //                }
        //            }
        //        }
        //        while (stack.Count > 0);
        //        if (urlList.Count > 0)
        //        {
        //            urlListStack.Push(urlList);
        //        }
        //    }
        //}

        private List<string> GetUrlListByUrl(string urlitem, Func<string, string> GetCompleteUrl, Func<string, bool> UrlIsMatch)
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
                AddUrl(curNode, urlList, GetCompleteUrl, (d) =>
                {
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

        //private static void AddUrl(List<string> distinctUrlList, string curUrl, List<string> urlList, HtmlNode node, string domain, string[] listPattern)
        //{
        //    AddUrl(node, urlList,
        //        (relativeUrl) =>
        //        {
        //            return Url.GetObsluteUrl(curUrl, relativeUrl);
        //        },
        //        (d) => {
        //        return string.IsNullOrWhiteSpace(d) == false && distinctUrlList.Contains(d) == false && d.StartsWith(domain)
        //            && IsMatchOne(d, listPattern);
        //        });            
        //}

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
