using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plant_Paradise.Models;

namespace Plant_Paradise.Controllers
{
    public class CategoriesAdminController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: CategoriesAdmin
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        [HttpPost]
        public ActionResult search(string search)
        {
            var Categories=db.Categories.Where(p=>p.Category_Name.Contains(search)).ToList();  
            return View("Index", Categories.ToList());
        }

        public ActionResult Statistics()
        {
            return View();
        }
        // GET: CategoriesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: CategoriesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Category_id,Category_Name,Category_Image")] Category category, HttpPostedFileBase Category_Image)
        {
            string fileName = fileName = Path.GetFileName(Category_Image.FileName);

            if (ModelState.IsValid)
            {
                string path = "/Images/" + Path.GetFileName(Category_Image.FileName);
                string path2 = Path.GetFileName(Category_Image.FileName);
                category.Category_Image = path2.ToString();
                Category_Image.SaveAs(Server.MapPath(path));


                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: CategoriesAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: CategoriesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(Include = "Category_id,Category_Name,Category_Image")] Category category, HttpPostedFileBase Category_Image)
        {
            if (ModelState.IsValid)
            {
                var existingModel = db.Categories.AsNoTracking().FirstOrDefault(x => x.Category_id == id);

                if (category != null)
                {
                    string path = "/Images/" + Path.GetFileName(Category_Image.FileName);
                    string path2 = Path.GetFileName(Category_Image.FileName);
                    Category_Image.SaveAs(Server.MapPath(path));
                    category.Category_Image = path2;
                }
                else
                {
                    category.Category_Image = existingModel.Category_Image;
                }

                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: CategoriesAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: CategoriesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
