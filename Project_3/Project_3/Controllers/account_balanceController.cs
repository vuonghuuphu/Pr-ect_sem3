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
    public class account_balanceController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: account_balance
        public ActionResult Index()
        {
            return View(db.account_Balances.ToList());
        }

        // GET: account_balance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account_balance account_balance = db.account_Balances.Find(id);
            if (account_balance == null)
            {
                return HttpNotFound();
            }
            return View(account_balance);
        }

        // GET: account_balance/Create
        public ActionResult Create()
        {
            HttpCookie cookie = Request.Cookies["Email"];
            var email = cookie.Values["emailuser"];
            var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
            ViewBag.id = data.Id;
            var bank = db.Banks.ToList();
            ViewBag.ListBank = bank;
            return View();
        }

        // POST: account_balance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_uer,Money")] account_balance account_balance)
        {
            if (ModelState.IsValid)
            {
                HttpCookie cookie = Request.Cookies["Email"];
                var email = cookie.Values["emailuser"];
                var data = db.Users.Where(s => s.Email == email).FirstOrDefault();
                var id = data.Id;
                var datapay = db.account_Balances.Where(s => s.Id_uer == id).FirstOrDefault();
                if (datapay == null) {
                db.account_Balances.Add(account_balance);
                db.SaveChanges();
                    return RedirectToAction("AccountManagement", "AuthUser");
                }
                else
                {
                    account_balance.Money = account_balance.Money + datapay.Money;
                    account_balance.Id = datapay.Id;
                    account_balance.Id_uer = datapay.Id_uer;
                    db.account_Balances.Remove(datapay);
                    db.account_Balances.Add(account_balance);
                    db.SaveChanges();
                    return RedirectToAction("AccountManagement", "AuthUser");
                }
                
            }

            return View();
        }

      

        // POST: account_balance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            account_balance account_balance = db.account_Balances.Find(id);
            db.account_Balances.Remove(account_balance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
