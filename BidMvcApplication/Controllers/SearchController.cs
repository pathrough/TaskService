using Pathrough.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Pathrough.LuceneSE;

namespace BidMvcApplication.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index(string kw)
        {
            List<BidMvcApplication.Models.Bid> model = new List<Models.Bid>();
            if (!string.IsNullOrWhiteSpace(kw))
            {
                //var db = new BidContext();
                //var list = (from item in db.Bids select item).ToList();
                var list = BidSearchEngine.Current.SearchContent(kw);
                model = list.Select(d => new BidMvcApplication.Models.Bid
                {
                    Title = d.BidTitle,
                    Content = d.BidContent,
                    Url = d.BidSourceUrl
                }).ToList();
                
                ViewBag.kw = kw;
            }
            return View(model);
        }

    }
}
