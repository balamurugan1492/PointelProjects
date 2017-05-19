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


namespace lucenceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string indexFileLocation = @"D:\Sample WorkOut Projects\Lucence.net\lucenceSample\index";

            Directory dir = FSDirectory.GetDirectory(indexFileLocation);

            //create an analyzer to process the text
            StandardAnalyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);


            //create the index writer with the directory and analyzer defined.
            IndexWriter indexWriter = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            //// //create a document, add in a single field
            Document doc = new Document();

            Field fldContent = new Field("contentname", "test3", Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
            Field flieldContent = new Field("firstname", "bala2", Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES);
            Field fieldCont = new Field("lastname", "murugan7", Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.YES);
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

                
                //to get all documents from directory
                for (int i = 0; i < reader.MaxDoc(); i++)
                {
                    Document documnet = reader.Document(i);
                    int j = documnet.GetFields().Count;
                    Console.WriteLine(documnet.GetField("firstname").StringValue().ToString() + "\t" + documnet.GetField("lastname").StringValue().ToString() + "\t");
                    //documnet = null;

                }
                //Console.ReadLine();
                
                
                
                //using queryparser 
                QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "lastname", analyzer);
                Query query = parser.Parse("murugan");

                //using wildcardquery 
                Term term=new Term("firstname","*a*");
                WildcardQuery query1 = new WildcardQuery(term);

                BooleanQuery boolQuery = new BooleanQuery();
                boolQuery.Add(query, BooleanClause.Occur.MUST);
                //boolQuery.Add(query, BooleanClause.Occur.MUST);
                Hits hits = indexSearcher.Search(boolQuery);
                if (hits.Length() != 0)
                {
                    for (int i = 0; i < hits.Length(); i++)
                    {
                        Document hitDoc = hits.Doc(i);
                        Console.WriteLine("\n"+hitDoc.GetField("firstname").StringValue().ToString() + "\t"+hitDoc.GetField("lastname").StringValue().ToString()+"\n");
                    }
                }
                else
                {
                    Console.WriteLine("there is no hits");
                }
                Console.ReadLine();
            }
        }
    }
}
