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
    public class BrandsController : Controller
    {
        private Datacontex db = new Datacontex();

        // GET: Brands
        public ActionResult Index(int? idcate)
        {
            ViewBag.ListCategory = db.categories.Where(p => p.Id == idcate).ToList();
            ViewBag.ListBrand = db.brands.ToList();
            return View();
        }
    }
}
