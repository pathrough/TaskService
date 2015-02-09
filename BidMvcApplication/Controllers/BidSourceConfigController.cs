using Pathrough.BLL;
using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication.Controllers
{
    public class BidSourceConfigController : Controller,IInsertHandler
    {
        IBidSourceConfigBLL bidSourceConfigBLL;
        public BidSourceConfigController()
        {
            bidSourceConfigBLL = new BidSourceConfigBLL();
        }
        public ActionResult Create(BidSourceConfig entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.ListUrl))
            {
                bidSourceConfigBLL.Insert(entity, this);
            }
            return View(entity);
        }      
        
        public ActionResult List()
        {
            int p = 0;
            int.TryParse(Request.QueryString["p"], out p);
            int pageCount, recordCount;
            var list = bidSourceConfigBLL.GetPageList(p, 20, out pageCount, out recordCount);
            return View(list);
        }

        public void ParameterInvalid()
        {
            throw new NotImplementedException();
        }


        public void EntityRepeat()
        {
            throw new NotImplementedException();
        }
    }
}
