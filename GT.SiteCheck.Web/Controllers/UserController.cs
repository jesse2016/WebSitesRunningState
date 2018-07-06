using GT.SiteCheck.BLL;
using GT.SiteCheck.BLL.impl;
using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace GT.SiteCheck.Web.Controllers
{
    public class UserController : Controller
    {
        UnityContainer container = new UnityContainer();
        IUserBLL IUser;

        public UserController()
        {
            container.RegisterType<IUserBLL, UserBLL>();
            IUser = container.Resolve<UserBLL>();
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public JsonResult Login(string userName, string Password)
        {
            string msg = string.Empty;
            bool isSuccess = false;
            try
            {
                User user = IUser.getUserByName(userName.Trim(), Password);

                if (user != null)
                {
                    Session["userName"] = user.username;
                    isSuccess = true;
                }
                else
                {
                    msg = "用户名和密码错误";
                }

                return Json(new { Result = isSuccess, Msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = isSuccess, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 退出登录
        public ActionResult Logout()
        {
            Session["userName"] = null;
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        #endregion
    }
}