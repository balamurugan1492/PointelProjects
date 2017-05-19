using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;

namespace Lucence.netSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //state the file location of the index
            string indexFileLocation = @"D:\Sample WorkOut Projects\Lucence.net\Lucence.netSample\Index";
          
            Directory dir =FSDirectory.Open(indexFileLocation);

            //create an analyzer to process the text
            StandardAnalyzer  analyzer=new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30); 
            

            //create the index writer with the directory and analyzer defined.
            IndexWriter indexWriter=new IndexWriter(dir,analyzer,IndexWriter.MaxFieldLength.UNLIMITED);

           // //create a document, add in a single field
           Document doc = new Document();

            Field fldContent =new Field("content","The quick brown fox jumps over the lazy dog",Field.Store.YES,Field.Index.NO,Field.TermVector.NO);
            Field flieldContent = new Field("firstname", "bala", Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
            Field fieldCont = new Field("lastname", "murugan", Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
            doc.Add(fldContent);
            doc.Add(flieldContent);
            doc.Add(fieldCont);
            //write the document to the index
            indexWriter.AddDocument(doc);
            //optimize and close the writer
            indexWriter.Optimize();
            indexWriter.Close();


            Console.WriteLine("if u want to view your lucence.net?(yes/no)");
            string result = Console.ReadLine();
            if (result == "yes")
            {
                 IndexReader reader = IndexReader.Open(dir, true);  
                IndexSearcher indexSearcher = new IndexSearcher(reader);
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "firstname", analyzer);
                 Query query = parser.Parse("bala");
                 TopScoreDocCollector collector = TopScoreDocCollector.Create(100, true);
                 ScoreDoc[] hits= collector.TopDocs().ScoreDocs;
                 for (int i = 0; i < hits.Length; i++)
                 {
                     Document hitDoc = indexSearcher.Doc(hits[i].Doc);
                    Console.WriteLine(hitDoc.GetField("firstname").StringValue.ToString()+"\n");
                 }
                 Console.ReadLine();
               
            }


        }
    }
}
