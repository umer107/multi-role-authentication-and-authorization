using FormAuthentication.Models;
using FormAuthentication.Models.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FormAuthentication.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {


            using (MVC_DBEntities context = new MVC_DBEntities())
            {
                bool IsValidUser = context.Users.Any(user => user.UserName.ToLower() ==
                     model.UserName.ToLower() && user.UserPassword == model.UserPassword);

                if (IsValidUser)
                {
                    var userRoles = (from user in context.Users
                                     join roleMapping in context.UserRolesMappings
                                     on user.ID equals roleMapping.UserID
                                     join role in context.RoleMasters
                                     on roleMapping.RoleID equals role.ID
                                     where user.UserName == model.UserName
                                     select user).ToList();

                    var jsSerializer = new JavaScriptSerializer();
                    var obj = jsSerializer.Serialize(userRoles);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe ?? false);
                    FormsAuthentication.SetAuthCookie(Convert.ToString(userRoles.Select(p => p.ID).FirstOrDefault()), model.RememberMe ?? false);
                    var authTicket = new FormsAuthenticationTicket(1, userRoles.Select(p => p.UserName).FirstOrDefault(), DateTime.Now, DateTime.Now.AddMinutes(20), false, obj);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("Index", "Employees");
                }

                ModelState.AddModelError("", "invalid Username or Password");
                return View();
            }
        }


        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User model)
        {
            using (MVC_DBEntities context = new MVC_DBEntities())
            {
                context.Users.Add(model);
                context.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}