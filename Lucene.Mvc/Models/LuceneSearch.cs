using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;
using System.IO;


namespace Lucene.Mvc.Models
{
    public static class LuceneSearch
    {
        private static string _luceneDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "lucene_index");
        private static FSDirectory _directoryTemp;
        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        private static void AddToLuceneIndex(SampleData sampleData, IndexWriter indexWriter)
        {
            //Remove older index entry
            TermQuery searchQuery = new TermQuery(new Term("Id", sampleData.Id.ToString()));
            indexWriter.DeleteDocuments(searchQuery);

            //add new index entry
            Document doc = new Document();

            //add Lucene fields mapped to database fields
            doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Category", sampleData.Category, Field.Store.YES, Field.Index.ANALYZED));

            //Add entry to index
            indexWriter.AddDocument(doc);
        }

        //Add list of records to search index
        public static void AddUpdateLuceneIndex(IEnumerable<SampleData> sampleDataList)
        {
            //Init Lucene
            StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (IndexWriter writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (SampleData sampleData in sampleDataList)
                {
                    AddToLuceneIndex(sampleData, writer);
                }
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(SampleData sampleData)
        {
            AddUpdateLuceneIndex(new List<SampleData> { sampleData });
        }

        //Remove Index Record from Lucene Index
        public static void ClearLuceneIndexRecord(int recordId)
        {
            StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (IndexWriter indexWriter = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                TermQuery searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                indexWriter.DeleteDocuments(searchQuery);

                //close handles
                analyzer.Close();
                indexWriter.Dispose();
            }
        }

        //Clear entire Index
        public static bool ClearLuceneIndex()
        {
            try
            {
                StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (IndexWriter indexWriter = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    indexWriter.DeleteAll();
                    analyzer.Close();
                    indexWriter.Dispose();
                }
            }
            catch (Exception exception)
            {
                return false;
            }
            return true;
        }

        //optimize large Lucene indexes
        public static void Optimize()
        {
            StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (IndexWriter indexWriter = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                indexWriter.Optimize();
                indexWriter.Dispose();
            }
        }

        //Map Index to data structure
        private static SampleData MapLuceneDocumentToData(Document doc)
        {
            return new SampleData
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                Name = doc.Get("Name"),
                Description = doc.Get("Description"),
                Category = doc.Get("Category")
            };
        }

        private static IEnumerable<SampleData> MapLuceneToDataList(
            IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<SampleData> MapLuceneToDataList(
            IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        public static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException parseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        //main search method
        private static IEnumerable<SampleData> InternalSearch(string searchQuery, string searchField)
        {
            //Empty query
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<SampleData>();

            //Setup Lucene Searcher
            using (IndexSearcher searcher = new IndexSearcher(_directory, false))
            {
                int hitLimit = 1000;
                StandardAnalyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);

                //Search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30,
                        searchField, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hitLimit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                //search by multiple fields
                else
                {
                    var parser = new MultiFieldQueryParser
                         (Version.LUCENE_30, new[] { "Id", "Name", "Description","Category" }, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hitLimit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        //public methods
        public static IEnumerable<SampleData> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<SampleData>();
            }

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);
            return InternalSearch(input, fieldName);
        }

        public static IEnumerable<SampleData> SearchDefault(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<SampleData>() : InternalSearch(input, fieldName);
        }
    }
}