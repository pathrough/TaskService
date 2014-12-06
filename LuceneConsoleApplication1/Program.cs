using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Pathrough.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Bid> bidList = new List<Bid> { 
            new Bid{ID=1,Title="盘古分词",BidContent="通过这个 Demo.exe 你可以对盘古分词的各种参数进行测试，你也可以点击保持配置来生成你在界面上设置好的参数的配置文件。"}
            };
            new Indexer().CreateIndexByData(bidList, (doc, bid) =>
            {
                doc.Add(new Field("id",bid.ID.ToString(),Field.Store.YES,Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title",bid.Title,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("content",bid.BidContent,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
            });
            string keyword = "盘古";
            var result  = new Searcher().SearchFromIndexData<Bid>(keyword, (entity, doc) =>
            {
                entity.Title = doc.Get("title");
                entity.BidContent = SplitContent.HightLight(keyword, doc.Get("content")); //搜索关键字高亮显示 使用盘古提供高亮插件
                entity.ID = Convert.ToInt32(doc.Get("id"));
            });
        }
    }
}
