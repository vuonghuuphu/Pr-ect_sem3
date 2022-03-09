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
    public class OderController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Admin/Oder
        public ActionResult Index()
        {
            return View(db.Oders.ToList());
        }

        // GET: Admin/Oder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oder oder = db.Oders.Find(id);
            if (oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }

        // GET: Admin/Oder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Oder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            if (ModelState.IsValid)
            {
                db.Oders.Add(oder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oder);
        }

        // GET: Admin/Oder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oder oder = db.Oders.Find(id);
            if (oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }

        // POST: Admin/Oder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Phone_number,Id_Brand,Id_Product,Price,Card_Id,Create_At")] Oder oder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oder);
        }

        // GET: Admin/Oder/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oder oder = db.Oders.Find(id);
            if (oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }

        // POST: Admin/Oder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Oder oder = db.Oders.Find(id);
            db.Oders.Remove(oder);
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
