using System.Web;
using System.Web.Mvc;

namespace BidMvcApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    public enum UserType
    {
        Common,
        Admin
    }

    public class UserAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public UserAuthorizeAttribute(UserType userType)
        {

        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userInfo = filterContext.HttpContext.Session[UserConfig.SESSION_NAME];
            if (userInfo == null)
            {
                filterContext.Result = new HttpUnauthorizedResult(); //返回未授权Result 
                //跳转到登录页面 
                filterContext.HttpContext.Response.Redirect("/User/Login");
            }
        }
    }

    public interface IUserView
    {
        void UserNoOrPasswordInvlid();
        void LoginSuccess();
    }

    public class UserAuthorize
    {
        public static void Login(string userNo,string userPwd,IUserView view)
        {
            if (userNo == "wangyb" && userPwd == "123456")
            {
                HttpContext.Current.Session[UserConfig.SESSION_NAME]=userNo;
                view.LoginSuccess();
            }           
            else
            {
                view.UserNoOrPasswordInvlid();
            }
        }
        public static void Logout()
        {
            HttpContext.Current.Session[UserConfig.SESSION_NAME] = null;
        }
    }

    public class UserConfig
    {
        public const string SESSION_NAME="userInfo";
    }
}