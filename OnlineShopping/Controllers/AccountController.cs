using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineShopping.Models;
namespace OnlineShopping.Controllers
{
    public class AccountController : Controller
    {
       
        User u = new User();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(Login model)
        {

            
            using (var context = new shoppingEntities())
            {
                //List<User> u = context.Users.Where(x => x.UserName == model.Username).ToList();
                //int id = u.Find(x => x.UserName == model.Username).userID;
                bool isvalid = context.Users.Any(x => x.UserName == model.Username && x.UserPassword == model.Password);
                
                if (isvalid)
                {
                    HttpCookie userIdCookie = new HttpCookie("uId");
                    userIdCookie.Value = model.Username;
                    userIdCookie.Expires = DateTime.Now.AddDays(10);
                    Response.SetCookie(userIdCookie);
                    FormsAuthentication.SetAuthCookie(model.Username, false);

                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        public ActionResult Signup()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User en)
        {
            using (shoppingEntities db = new shoppingEntities())
            {
                db.Users.Add(en);
                db.SaveChanges();

            }
            return RedirectToAction("login");
        }

        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reset(Reset model)
        {
            using (var context = new shoppingEntities())
            {
                bool isvalid = context.Users.Any(x => x.UserName == model.Username);
                if (isvalid)
                {
                    var cols = context.Users.Where(x => x.UserName.Equals(model.Username));
                    foreach (User c in cols)
                    {
                        c.UserPassword = model.NewPassword;

                    }
                    context.SaveChanges();

                    return RedirectToAction("login");
                }
                ModelState.AddModelError("", "Username does not exist!! ");
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }

    }
}

