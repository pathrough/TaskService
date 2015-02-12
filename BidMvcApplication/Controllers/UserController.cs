using BidMvcApplication.Models;
using Pathrough.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication.Controllers
{
    public class UserController : Controller, IUserView
    {       
        public ActionResult Login(string userNo,string userPwd)
        {
            if (!string.IsNullOrWhiteSpace(userNo) && !string.IsNullOrWhiteSpace(userPwd))
            {
                UserAuthorize.Login(userNo, userPwd, this);    
            }                  
            return View();
        }

        public void UserNoOrPasswordInvlid()
        {
            Json(new JsonResultEntity { Result = 2, Msg = "用户名或密码错误！" }).ExecuteResult(this.ControllerContext);
            //Response.Write(new JsonResultEntity { Result = 2, Msg = "用户名或密码错误！" }.ToJson());
            Response.End();
        }

        public void LoginSuccess()
        {
            Json(new JsonResultEntity { Result = 1, Msg = "登录成功！" }).ExecuteResult(this.ControllerContext);
            //Response.Write();
            Response.End();
        }
    }
}
