using Pathrough.EF;
using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            var db = new BidContext();
            return View();
        }

        public string AddBid()
        {
            var db = new BidContext();
            db.Bids.Add(new Bid { BidTitle="标题"});
            db.SaveChanges();
            return "success";
        }

    }
}
