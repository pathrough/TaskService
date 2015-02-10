using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication.Controllers
{
    public class UserController : Controller, IUserView
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Login(string userNo,string userPwd)
        {
            if(Request.Form.Count>0 | Request.QueryString.Count>0)
            {
                UserAuthorize.Login(userNo, userPwd, this);
            }            
            return View();
        }


        //
        // GET: /User/Details/5
        [UserAuthorize(UserType.Common)]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

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
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

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

        public void UserNoOrPasswordInvlid()
        {
            Response.Write("用户名或密码错误！");
            Response.End();
        }

        public void LoginSuccess()
        {
            Response.Write("登录成功");
            Response.End();
        }
    }
}
