using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Plant_Paradise.Models;

namespace Plant_Paradise.Controllers
{
    public class Sub_CategoriesAdminController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: Sub_CategoriesAdmin
        public ActionResult Index(int id)
        {
            Session["Categoryid"] = id;
            var sub_Categories = db.Sub_Categories.Include(s => s.Category).Where(p=>p.Category_id== id);
            return View(sub_Categories.ToList());
        }
        [HttpPost]
        public ActionResult search(string Search)
        {
            int id = int.Parse(Session["Categoryid"].ToString());
            var sub_Categories = db.Sub_Categories.Include(s => s.Category).Where(p => p.SubCategoryName.Contains(Search) && p.Category_id==id);
            return View("Index", sub_Categories.ToList());
        }
        // GET: Sub_CategoriesAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sub_Categories sub_Categories = db.Sub_Categories.Find(id);
            if (sub_Categories == null)
            {
                return HttpNotFound();
            }
            return View(sub_Categories);
        }

        // GET: Sub_CategoriesAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name");
            return View();
        }

        // POST: Sub_CategoriesAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategory_id,Category_id,SubCategoryImage,SubCategoryName,SubCategory_Description")] Sub_Categories sub_Categories, HttpPostedFileBase SubCategoryImage)
        {

            string fileName = fileName = Path.GetFileName(SubCategoryImage.FileName);

            if (ModelState.IsValid)
            {
                string path = "/Images/" + Path.GetFileName(SubCategoryImage.FileName);
                string path2 = Path.GetFileName(SubCategoryImage.FileName);
                sub_Categories.SubCategoryImage = path2.ToString();
                SubCategoryImage.SaveAs(Server.MapPath(path));

                db.Sub_Categories.Add(sub_Categories);
                db.SaveChanges();
                return RedirectToAction( "Index", "Sub_CategoriesAdmin", new { id = Session["Categoryid"] });
            }

            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", sub_Categories.Category_id);
            return View(sub_Categories);
        }

        // GET: Sub_CategoriesAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sub_Categories sub_Categories = db.Sub_Categories.Find(id);
            if (sub_Categories == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", sub_Categories.Category_id);
            return View(sub_Categories);
        }

        // POST: Sub_CategoriesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(Include = "SubCategory_id,Category_id,SubCategoryImage,SubCategoryName,SubCategory_Description")] Sub_Categories sub_Categories, HttpPostedFileBase SubCategoryImage)
        {
            if (ModelState.IsValid)
            {
                var existingModel = db.Sub_Categories.AsNoTracking().FirstOrDefault(x => x.SubCategory_id == id);

                if (SubCategoryImage != null)
                {
                    string path = "/Images/" + Path.GetFileName(SubCategoryImage.FileName);
                    string path2 = Path.GetFileName(SubCategoryImage.FileName);
                    SubCategoryImage.SaveAs(Server.MapPath(path));
                    sub_Categories.SubCategoryImage = path2;
                }
                else
                {
                    sub_Categories.SubCategoryImage = existingModel.SubCategoryImage;
                }
                db.Entry(sub_Categories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Sub_CategoriesAdmin", new { id = Session["Categoryid"] });
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", sub_Categories.Category_id);
            return View(sub_Categories);
        }

        // GET: Sub_CategoriesAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sub_Categories sub_Categories = db.Sub_Categories.Find(id);
            if (sub_Categories == null)
            {
                return HttpNotFound();
            }
            return View(sub_Categories);
        }

        // POST: Sub_CategoriesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sub_Categories sub_Categories = db.Sub_Categories.Find(id);
            db.Sub_Categories.Remove(sub_Categories);
            db.SaveChanges();
            return RedirectToAction("Index", "Sub_CategoriesAdmin", new { id = Session["Categoryid"] });
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
