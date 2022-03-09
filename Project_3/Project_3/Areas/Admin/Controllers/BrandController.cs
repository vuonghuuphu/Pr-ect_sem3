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
    public class BrandController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Admin/Brand
        public ActionResult Index()
        {
            return View(db.brands.ToList());
        }

        // GET: Admin/Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Brand brand, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                string catImg = "~/Uploads/default.png"; //edit -  category.Image
                try
                {
                    if (Img != null)
                    {
                        string fileName = Path.GetFileName(Img.FileName);// lay url path khi upload len
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
                    brand.Img = catImg;
                }
                db.brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Admin/Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Brand brand, HttpPostedFileBase Img)
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
                    brand.Img = catImg;
                }

                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Admin/Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.brands.Find(id);
            db.brands.Remove(brand);
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
