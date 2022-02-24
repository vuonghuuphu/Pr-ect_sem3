using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Project_3.Models;


namespace Project_3.Controllers
{
    public class AuthUserController : Controller
    {
        // GET: AuthUserController
        private Datacontex db = new Datacontex();
        // GET: AuthUser
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ViewBag.Error = "Email này đã tồn tại";
                }
            }
            return View();
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] frData = Encoding.UTF8.GetBytes(str);
            byte[] toData = md5.ComputeHash(frData);
            string hashString = "";
            for (int i = 0; i < toData.Length; i++)
            {
                hashString += toData[i].ToString("x2");
            }
            return hashString;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                login.Password = GetMD5(login.Password);
                var data = db.Users.Where(s => s.Email.Equals(login.Email) && s.Password.Equals(login.Password)).FirstOrDefault();
                if (data != null)
                {
                    FormsAuthentication.SetAuthCookie(data.Email, true);

                HttpCookie cookie = new HttpCookie("Email");
                cookie.Values["emailuser"] = data.Email;
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);
                    return Redirect("~/Home/Index");
                }
               
            }
            return View();
        }

        public ActionResult AccountManagement() {

            HttpCookie cookie = Request.Cookies["Email"];

            if ((cookie.Values["emailuser"] != null) && (cookie.Values["emailuser"] != ""))
            {
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                ViewBag.user = email;
                var datamoney = db.account_Balances.Where(s => s.Id_uer == data.Id).FirstOrDefault();
                if (datamoney == null) {
                    ViewBag.datamoney = 0;
                }
                else
                {
                    ViewBag.datamoney = datamoney.Money.ToString() ;
                }
                
                return View();
            }
            else
            {
                return RedirectToAction("Login", "AuthUser");
            }
        }


        public ActionResult Logout()
        {
            Session.Clear();
            HttpCookie cookie = Request.Cookies["Email"];
            cookie.Values["emailuser"] = null;
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            if (cookie != null)
            {
                return RedirectToAction("AccountManagement", "AuthUser");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        }
}
