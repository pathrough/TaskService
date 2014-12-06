using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
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
    public class Indexer
    {
        public void CreateIndexByData<T>(List<T> list, Action<Document, T> AddField)
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexConfig.IndexDirectory), new NativeFSLockFactory());
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
           
            foreach (var item in list)
            {
                Document doc = new Document();
                ////Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                ////WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离
                //doc.Add(new Field("id",bid.ID.ToString(),Field.Store.YES,Field.Index.NOT_ANALYZED));
                //doc.Add(new Field("title",bid.Title,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                //doc.Add(new Field("content",bid.BidContent,Field.Store.YES,Field.Index.ANALYZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
                AddField(doc, item);
                writer.AddDocument(doc);
            }
            writer.Close();
            directory.Close();
        }
    }
}
