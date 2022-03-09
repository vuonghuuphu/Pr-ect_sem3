using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_3.Models;

namespace Project_3.Areas.Admin.Controllers
{
    public class order_adminController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Admin/order_admin
        public ActionResult Order_admin()
        {

            var queryo = "SELECT * FROM dbo.Oders ORDER BY Id DESC";
            ViewBag.ListBill = db.Oders.SqlQuery(queryo).ToList();
            ViewBag.ListBill2 = "1";
            return View();
        }
        [HttpPost]
        public ActionResult Order_admin(Day day)
        {
            var day1 = day.Date;
           
            if (day.Date == null && day.Month == null && day.Year == 0)
            {
                ViewBag.ListBill1 = day.Month;
                var queryo = "SELECT * FROM dbo.Oders ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(queryo).ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '___%" + day.Date + "%____%' ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '___%" + day.Date + "%" + day.Year + "%' ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%____%' ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%" + day.Year + "%' ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%'  ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Year + "%'  ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Year + "%'  ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Year + "%'  ORDER BY Id DESC";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            ViewBag.ListBill1 = day.Month;

            return View();
        }
    
}
}