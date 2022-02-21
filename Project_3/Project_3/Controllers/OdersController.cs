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
    }
     
}
