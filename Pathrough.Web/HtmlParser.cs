using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Web
{
    public class HtmlParser
    {
        HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
        public HtmlParser(string html)
        {
            doc.LoadHtml(html);
        }

        public HtmlParser(HtmlDocument doc)
        {
            this.doc = doc;
        }

        public List<string> GetUrlList()
        {
           return GetUrlList("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUrl">当前页面的url，用来修正相对地址</param>
        /// <returns></returns>
        public List<string> GetUrlList(string currentUrl)
        {
            return this.AllLinkTextList.Select(d => d.Key).ToList();
        }

        void GetUrlList(HtmlNode node,ref List<KeyValuePair<string,string>> linkKvList, string currentUrl)
        {
            if (linkKvList == null)
            {
                linkKvList = new List<KeyValuePair<string,string>>();
            }
            var atts = node.Attributes;
            foreach (var att in atts)
            {
                if (att.Name == "href")
                {
                    linkKvList.Add(new KeyValuePair<string,string>(Url.GetObsluteUrl(currentUrl, att.Value),att.OwnerNode.InnerText));
                }
            }
            foreach (var child in node.ChildNodes)
            {
                GetUrlList(child, ref linkKvList, currentUrl);
            }
        }

        List<KeyValuePair<string, string>> _AllLinkTextList;
        public List<KeyValuePair<string,string>> AllLinkTextList
        {
            get
            {
                if(_AllLinkTextList==null)
                {
                    _AllLinkTextList = new List<KeyValuePair<string, string>>();
                    GetUrlList(doc.DocumentNode, ref _AllLinkTextList, "");
                }
                return _AllLinkTextList;
            }
        }       
    }
}
