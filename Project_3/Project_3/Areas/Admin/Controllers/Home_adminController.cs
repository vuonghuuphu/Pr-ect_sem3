using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Project_3.Models;

namespace Project_3.Areas.Admin.Controllers
{
    public class Home_adminController : Controller
    {
        private Datacontex db = new Datacontex();
        // GET: Admin/Home_admin
        public ActionResult Index()
        {
            var total = 0;
            var queryt = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + DateTime.Now.Year + "%'";
            var ca = db.Oders.SqlQuery(queryt).ToList();
            foreach (var it in ca) {
                total = total + it.Price;
            }
            ViewBag.total = total;

            ViewBag.tk1 = db.categories.Count();
            ViewBag.tk2 = db.brands.Count();
            ViewBag.tk3 = db.Users.Count();
            ViewBag.tk4 = db.Oders.Count();
            var Year = DateTime.Now.Year;
            var query1 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Jan" + "%" + Year + "%'";
            ViewBag.o1 = db.Oders.SqlQuery(query1).Count();
            var query2 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Feb" + "%" + Year + "%'";
            ViewBag.o2 = db.Oders.SqlQuery(query2).Count();
            var query3 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Mar" + "%" + Year + "%'";
            ViewBag.o3 = db.Oders.SqlQuery(query3).Count();
            var query4 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Apr" + "%" + Year + "%'";
            ViewBag.o4 = db.Oders.SqlQuery(query4).Count();
            var query5 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "May" + "%" + Year + "%'";
            ViewBag.o5 = db.Oders.SqlQuery(query5).Count();
            var query6 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Jun" + "%" + Year + "%'";
            ViewBag.o6 = db.Oders.SqlQuery(query6).Count();
            var query7 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Jul" + "%" + Year + "%'";
            ViewBag.o7 = db.Oders.SqlQuery(query7).Count();
            var query8 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Aug" + "%" + Year + "%'";
            ViewBag.o8 = db.Oders.SqlQuery(query8).Count();
            var query9 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Sep" + "%" + Year + "%'";
            ViewBag.o9 = db.Oders.SqlQuery(query9).Count();
            var query10 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Oct" + "%" + Year + "%'";
            ViewBag.o10 = db.Oders.SqlQuery(query10).Count();
            var query11 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Nov" + "%" + Year + "%'";
            ViewBag.o11 = db.Oders.SqlQuery(query11).Count();
            var query12 = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + "Dec" + "%" + Year + "%'";
            ViewBag.o12 = db.Oders.SqlQuery(query12).Count();
            return View();
        }
    }
}