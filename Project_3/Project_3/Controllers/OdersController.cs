using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_3.Models;

namespace Project_3.Controllers
{
    public class OdersController : Controller
    {

        private Datacontex db = new Datacontex();

        // GET: Oders
        public ActionResult Index(int? p, int? d)
        {
            ViewBag.ListProduct = p;
            ViewBag.ListBrand = d;
            return View();
        }


        // GET: Oders/Create
        public ActionResult Create(int? c, int? br, int? pro, int? pho)
        {
            if (br == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var brand = db.brands.Where(p => p.Id == br).ToList();
            var product = db.products.Where(p => p.Id == pro).ToList();
            var bank = db.Banks.ToList();
            if (brand == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ListProduct = product.First().Name;
                ViewBag.ListProductid = product.First().Id;
                ViewBag.ListBrand = brand.First().Name;
                ViewBag.ListBrandid = brand.First().Id;
                ViewBag.ListPrice = product.First().Price;
                ViewBag.ListBank = bank;
                ViewBag.Phone = pho;
            }
            Oder model = new Oder();
            model.Id_Brand = (int)br;
            model.Id_Product = (int)pro;
            model.Phone_number = pho.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            oder.Create_At = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Oders.Add(oder);
                db.SaveChanges();

                return RedirectToAction("Index", "Oders", new { p = oder.Id, d = oder.Create_At});
            }

            return RedirectToAction("Create", "Oders", new { br = oder.Id_Brand, pro = oder.Id_Product, pho = oder.Phone_number }) ;
        }


        public ActionResult Bill(int? p)
        {
            var bill = db.Oders.Where(b => b.Id == p).ToList();
            ViewBag.ListOder = bill;
            var idbrand = bill.First().Id_Brand;
            var bra = db.brands.Where(b => b.Id == idbrand).ToList();
            ViewBag.ListBrand = bra;
            var idpro = bill.First().Id_Product;
            var pro = db.products.Where(b => b.Id == idpro).ToList();
            ViewBag.ListProduct = pro;
            return View();
        }

        public ActionResult Oder_account( int? br, int? pro)
        {
            HttpCookie cookie = Request.Cookies["Email"];
            if (cookie == null) { 
            return RedirectToAction("Login", "AuthUser");
            }
            var email = cookie.Values["emailuser"];
            if(email == "")
            {
                return RedirectToAction("Login", "AuthUser");
            }

            else { 
            if (br == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();
                var brand = db.brands.Where(p => p.Id == br).ToList();
                var product = db.products.Where(p => p.Id == pro).ToList();
                if (datapay.Money < product.First().Price) {
                    return RedirectToAction("AccountManagement", "AuthUser");
                }
                if (brand == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    ViewBag.Money = datapay.Money;
                    ViewBag.ListProduct = product.First().Name;
                    ViewBag.ListProductid = product.First().Id;
                    ViewBag.ListBrand = brand.First().Name;
                    ViewBag.ListBrandid = brand.First().Id;
                    ViewBag.ListPrice = product.First().Price;
                }
                Oder model = new Oder();
                model.Id_Brand = (int)br;
                model.Id_Product = (int)pro;
            return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Oder_account([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            oder.Create_At = DateTime.Now;
            oder.Card_Id = 0;
            if (ModelState.IsValid)
            {

                HttpCookie cookie = Request.Cookies["Email"];
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();

                account_balance a = new account_balance();
                a.Id = datapay.Id;
                a.Id_uer = datapay.Id_uer;
                a.Money = datapay.Money - oder.Price;

                db.account_Balances.Remove(datapay);
                db.account_Balances.Add(a);
                db.SaveChanges();

                db.Oders.Add(oder);
                db.SaveChanges();

                return RedirectToAction("Bill_Oder_account", "Oders", new { id = oder.Id,pho = oder.Phone_number});
            }

            return RedirectToAction("Create", "Oders", new { br = oder.Id_Brand, pro = oder.Id_Product,  });
        }

        public ActionResult Bill_Oder_account(int? id)
        {
            var bill = db.Oders.Where(b => b.Id == id).ToList();
            ViewBag.ListOder = bill;
            var idbrand = bill.First().Id_Brand;
            var bra = db.brands.Where(b => b.Id == idbrand).ToList();
            ViewBag.ListBrand = bra;
            var idpro = bill.First().Id_Product;
            var pro = db.products.Where(b => b.Id == idpro).ToList();
            ViewBag.ListProduct = pro;
            return View();
        }


        public ActionResult oder_admin()
        {
            ViewBag.ListBill = db.Oders.ToList();
            ViewBag.ListBill2 = "1";
            return View();
        }
        [HttpPost]
        public ActionResult oder_admin(Day day)
        {
            if (day.Date != null)
            {
                int result = Int32.Parse(day.Date);
                if (result < 10)
                {
                    day.Date = "0" + day.Date;
                }
            }
            if (day.Date == null && day.Month == null && day.Year == 0)
            {
                ViewBag.ListBill1 = day.Month;
                ViewBag.ListBill = db.Oders.ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '____"+day.Date+"_____%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Date + "%" + day.Year + "%__%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date != null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Date + "%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year == 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%__%" + day.Year + "%__%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month != null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Month + "%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            else if (day.Date == null && day.Month == null && day.Year != 0)
            {
                var query = "SELECT * FROM dbo.Oders WHERE Create_At LIKE '%" + day.Year + "%'";
                ViewBag.ListBill = db.Oders.SqlQuery(query).ToList();
            }
            ViewBag.ListBill1 = day.Month;

            return View();
        }
    }

} 
