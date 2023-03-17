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
    public class ProductsViewAdminController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: Products1
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Sub_Categories);
            return View(products.ToList());
        }
        [HttpPost]
        public ActionResult search(string search, int plants = 0)
        {

            var products = db.Products.Include(p => p.Sub_Categories);

            if (plants != 0)
            {
                products = db.Products.Include(p => p.Sub_Categories).Where(p => p.Sub_Categories.Category_id == plants && p.Product_Name.Contains(search));
            }
            else
            {
                products = db.Products.Include(p => p.Sub_Categories).Where(p => p.Product_Name.Contains(search));

            }
            return View("Index", products.ToList());
        }
        // GET: Products1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products1/Create
        public ActionResult Create()
        {
            ViewBag.Subcategory_id = new SelectList(db.Sub_Categories, "SubCategory_id", "SubCategoryName");
            return View();
        }

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_id,Product_Name,Product_Image,Product_Size,Product_Color,Product_Price,Quantity,Product_Description,Subcategory_id,Water_needed")] Product product, HttpPostedFileBase  Product_Image)
        {
            string fileName = fileName = Path.GetFileName(Product_Image.FileName);

            if (ModelState.IsValid)
            {
                string path = "/Images/" + Path.GetFileName(Product_Image.FileName);
                string path2 = Path.GetFileName(Product_Image.FileName);
                product.Product_Image= path2.ToString();

                Product_Image.SaveAs(Server.MapPath(path));

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Subcategory_id = new SelectList(db.Sub_Categories, "SubCategory_id", "SubCategory_Description", product.Subcategory_id);
            return View(product);
        }

        // GET: Products1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Subcategory_id = new SelectList(db.Sub_Categories, "SubCategory_id", "SubCategoryName", product.Subcategory_id);
            return View(product);
        }

        // POST: Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(Include = "Product_id,Product_Name,Product_Image,Product_Size,Product_Color,Product_Price,Quantity,Product_Description,Subcategory_id,Water_needed")] Product product, HttpPostedFileBase Product_Image)
        {
            if (ModelState.IsValid)
            {

                var existingModel = db.Products.AsNoTracking().FirstOrDefault(x => x.Product_id == id);

                if (Product_Image != null)
                {
                    string path = "/Images/" + Path.GetFileName(Product_Image.FileName);
                    string path2 = Path.GetFileName(Product_Image.FileName);
                    Product_Image.SaveAs(Server.MapPath(path));
                    product.Product_Image = path2;
                }
                else
                {
                    product.Product_Image = existingModel.Product_Image;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Subcategory_id = new SelectList(db.Sub_Categories, "SubCategory_id", "SubCategory_Description", product.Subcategory_id);
            return View(product);
        }

        // GET: Products1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
