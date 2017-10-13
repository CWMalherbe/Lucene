using Lucene.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Lucene.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateIndex()
        {
            LuceneSearch.ClearLuceneIndex();
            var x = SampleDataRepository.GetAll();
            LuceneSearch.AddUpdateLuceneIndex(x);
            return RedirectToAction("Index");
        }

        public ActionResult Search(FormCollection formCollection)
        {
            string searchTerm = formCollection["Search"];
            IEnumerable<SampleData> results = LuceneSearch.Search(searchTerm, string.Empty);
            return View(results);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}