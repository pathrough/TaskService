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
        
        public ActionResult List(string areaNo,int pageIndex,int pageSize)
        {
            int pageCount, recordCount;
            bidSourceConfigBLL.GetList(areaNo, pageIndex, pageSize, out pageCount, out recordCount);
            return View();
        }

        public void ParameterInvalid()
        {
            
        }
    }
}
