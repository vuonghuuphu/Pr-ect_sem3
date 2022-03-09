using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_3.Models;
using System.IO;

namespace Project_3.Areas.Admin.Controllers
{
    public class CategorieController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Admin/Categorie
        public ActionResult Index()
        {
            return View(db.categories.ToList());
        }

        // GET: Admin/Categorie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.categories.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // GET: Admin/Categorie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categorie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Categorie categorie, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                string catImg = "~/Uploads/default.png";
                try
                {
                    if (Img != null)
                    {
                        string fileName = Path.GetFileName(Img.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        Img.SaveAs(path);
                        catImg = "~/Uploads/" + fileName;
                    }

                }
                catch (Exception e)
                {
                }
                finally
                {
                    categorie.Img = catImg;
                }

                db.categories.Add(categorie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categorie);
        }

        // GET: Admin/Categorie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.categories.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // POST: Admin/Categorie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Categorie categorie, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                string catImg = "~/Uploads/default.png";
                try
                {
                    if (Img != null)
                    {
                        string fileName = Path.GetFileName(Img.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        Img.SaveAs(path);
                        catImg = "~/Uploads/" + fileName;
                    }

                }
                catch (Exception e)
                {
                }
                finally
                {
                    categorie.Img = catImg;
                }

                db.Entry(categorie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categorie);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categorie categorie = db.categories.Find(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }
        // POST: Admin/Categorie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categorie categorie = db.categories.Find(id);
            db.categories.Remove(categorie);
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
