using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Pathrough.Lucene.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.Lucene
{
    public class Searcher
    {
        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        public List<T> SearchFromIndexData<T>(string kw, Action<T, Document> AddResult) where T : new()
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexConfig.IndexDirectory), new NoLockFactory());
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
