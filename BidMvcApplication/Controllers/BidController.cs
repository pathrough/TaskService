using Pathrough.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication.Controllers
{
    public class BidController : Controller
    {
        //
        // GET: /Bid/
        BidBLL bid = new BidBLL();
        public ActionResult Index()
        {            
            int p=0;
            int.TryParse( Request.QueryString["p"],out p);
            int pageCount, recordCount;
            var result = bid.GetPageList(p, 20, out pageCount, out recordCount);
            return View(result);
        }

        //
        // GET: /Bid/Details/5

        public ActionResult Details(long id)
        {
            var entity = bid.GetEntity(id);
            return View(entity);
        }


        //
        // GET: /Bid/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Bid/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Bid/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Bid/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Bid/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Bid/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
