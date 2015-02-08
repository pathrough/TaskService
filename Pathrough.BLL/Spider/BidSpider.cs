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

            string listUrl = config.ListUrl;
            List<Bid> bidList = new List<Bid>();
            string domain = new Url(listUrl).DomainUrl;
            var list = GetDetailUrlListByUrl(listUrl, config.DetailUrlPattern.Split(new string[] { "-/-" }, StringSplitOptions.RemoveEmptyEntries));
            ExceptionBidSourceConfigBLL ebsc = new ExceptionBidSourceConfigBLL();
            bool isRecordException = false;
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    try
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
                                if (!isRecordException)
                                {
                                    ebsc.Insert(new ExceptionBidSourceConfig { Config_BscID = config.BscID, Msg = "BidTitle，根据xPath获取时失败！", LogDate = DateTime.Now });
                                    isRecordException = true;
                                }

                                throw e;
                            }
                            string strPubTime = "";
                            try
                            {
                                strPubTime = doc.DocumentNode.SelectSingleNode(config.PubishDateXpath).InnerText;
                            }
                            catch (Exception e)
                            {
                                if (!isRecordException)
                                {
                                    //todo:记录获取失败
                                    ebsc.Insert(new ExceptionBidSourceConfig { Config_BscID = config.BscID, Msg = "BidPublishDate，根据xPath获取时失败！", LogDate = DateTime.Now });
                                    isRecordException = true;
                                }
                                throw e;
                            }
                            var m = Regex.Match(strPubTime, config.PubishDatePattern);
                            if (m.Success)
                            {
                                bid.BidPublishDate = GetDateTime(m.Groups[1].Value);
                            }
                            else
                            {
                                if (!isRecordException)
                                {
                                    ebsc.Insert(new ExceptionBidSourceConfig { Config_BscID = config.BscID, Msg = "BidPublishDate，转换失败！", LogDate = DateTime.Now });
                                    isRecordException = true;
                                }
                            }
                            try
                            {
                                bid.BidContent = doc.DocumentNode.SelectSingleNode(config.ContentXpath).InnerText;
                            }
                            catch (Exception e)
                            {
                                if (!isRecordException)
                                {
                                    //todo:记录获取失败
                                    ebsc.Insert(new ExceptionBidSourceConfig { Config_BscID = config.BscID, Msg = "BidContent，根据xPath获取时失败！", LogDate = DateTime.Now });
                                    isRecordException = true;
                                }
                                throw e;
                            }

                            bidList.Add(bid);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                if (!isRecordException)
                {
                    ebsc.Insert(new ExceptionBidSourceConfig { Config_BscID = config.BscID, Msg = "无法获取列表", LogDate = DateTime.Now });
                    isRecordException = true;
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

        private List<string> GetUrlListByUrl(string urlitem, Func<string, string> GetCompleteUrl, Func<string, bool> UrlIsMatch)
        {
            HtmlDocument doc = new WebPageLoader().GetPage(urlitem);

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

        private List<string> GetUrlListByUrl(string urlitem, string[] detailPatterns)
        {
            HtmlDocument doc = new WebPageLoader().GetPage(urlitem);

            //列表页，url分析
            List<string> urlList = new List<string>();

            Stack<HtmlNode> stack = new Stack<HtmlNode>();
            stack.Push(doc.DocumentNode);
            do
            {
                var curNode = stack.Pop();
                AddUrl(curNode, urlList, detailPatterns);
                if (curNode.HasChildNodes)
                {
                    foreach (var node in curNode.ChildNodes)
                    {
                        stack.Push(node);
                        AddUrl(node, urlList, detailPatterns);
                    }
                }
            }
            while (stack.Count > 0);
            return urlList;
        }

        private List<string> GetDetailUrlListByUrl(string urlitem, string[] detailPattern, string domain)
        {
            return GetUrlListByUrl(urlitem, (d) => d /*(relativeUrl) =>
            {
                return Url.GetObsluteUrl(urlitem, relativeUrl);
            }*/, (d) =>
            {
                return string.IsNullOrWhiteSpace(d) == false
                    && IsMatchOne(d, detailPattern);
            });
        }

        private List<string> GetDetailUrlListByUrl(string urlitem, string[] detailPattern)
        {
            return GetUrlListByUrl(urlitem, detailPattern);
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

        private static string AddUrl(HtmlNode node, List<string> urlList, string[] detailPatterns)
        {
            if (node.Name == "a")
            {
                if (node.HasAttributes)
                {
                    foreach (var attr in node.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            string url = attr.Value;
                            if (string.IsNullOrWhiteSpace(attr.Value) == false
                    && IsMatchOne(url, detailPatterns) && urlList.Contains(url) == false)
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
