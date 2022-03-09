using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_3.Models;

namespace Project_3.Areas.Admin.Controllers
{
    public class account_balancesController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Admin/account_balances
        public ActionResult Index()
        {
            return View(db.account_Balances.ToList());
        }

        // GET: Admin/account_balances/Details/5
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
            ViewBag.user = db.Users.Where(u => u.Id == account_balance.Id_uer).FirstOrDefault().Email;
            return View(account_balance);
        }

        // GET: Admin/account_balances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/account_balances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_uer,Money")] account_balance account_balance)
        {
            if (ModelState.IsValid)
            {
                db.account_Balances.Add(account_balance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account_balance);
        }

        // GET: Admin/account_balances/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Admin/account_balances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_uer,Money")] account_balance account_balance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account_balance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account_balance);
        }

        // GET: Admin/account_balances/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/account_balances/Delete/5
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
