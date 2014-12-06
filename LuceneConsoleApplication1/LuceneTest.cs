using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneConsoleApplication1
{
    public class Bid
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string BidContent { get; set; }
    }

    public class SplitContent
    {
        public static string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            Lucene.Net.Analysis.Token token = null;
            while ((token = tokenStream.Next()) != null)
            { //Next继续分词 直至返回null
                strList.Add(token.TermText()); //得到分词后结果
            }
            return strList.ToArray();
        }

        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        public static string HightLight(string keyword, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }

    public class LuceneTest
    {
        string indexPath = "IndexData";
        public void CreateIndexByData<T>(List<T> list, Action<Document, T> AddField)
        {            
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            //List<Bid> bidList = new List<Bid> { 
            //new Bid{ID=1,Title="盘古分词",BidContent="通过这个 Demo.exe 你可以对盘古分词的各种参数进行测试，你也可以点击保持配置来生成你在界面上设置好的参数的配置文件。"}
            //};
            foreach (var item in list)
            {
                Document doc = new Document();
                ////Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                ////WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                //doc.Add(new Field("id",bid.ID.ToString(),Field.Store.YES,Field.Index.NOT_ANALYZED));
                //doc.Add(new Field("title",bid.Title,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                //doc.Add(new Field("content",bid.BidContent,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                AddField(doc,item);
                writer.AddDocument(doc);
            }
            writer.Close();
            directory.Close();
        }

        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        public List<T> SearchFromIndexData<T>(string kw, Action<T, Document> AddResult) where T:new()
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            
            PhraseQuery query = new PhraseQuery();//搜索条件

            //把用户输入的关键字进行分词
            foreach (string word in SplitContent.SplitWords(kw))
            {
                query.Add(new Term("content", word));//多个查询条件时 为且的关系
            }
            query.SetSlop(100); //指定关键词相隔最大距离

          
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);  //TopScoreDocCollector盛放查询结果的容器
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;//TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果

            //展示数据实体对象集合
            List<T> bookResult = new List<T>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document
                T entity = new T();
                AddResult(entity, doc);
                bookResult.Add(entity);
                //Bid book = new Bid();
                //book.Title = doc.Get("title");               
                //book.BidContent = SplitContent.HightLight(kw, doc.Get("content")); //搜索关键字高亮显示 使用盘古提供高亮插件
                //book.ID = Convert.ToInt32(doc.Get("id"));
                //bookResult.Add(book);
            }
            return bookResult;
        }
    }
}
