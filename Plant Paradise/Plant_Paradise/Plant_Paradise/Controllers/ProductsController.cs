using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Plant_Paradise.Models;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Plant_Paradise.Controllers
{
    public class ProductsController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: Products
        public ActionResult Index(int id)
        {
            var products = db.Products.Include(p => p.Sub_Categories).Where(p=>p.Subcategory_id==id).OrderByDescending(p=>p.Product_id) ;
            return View(products.ToList());
        }
       

             public ActionResult SingleProduct(int id)
        {
            var products = db.Products.Include(p => p.Sub_Categories).Where(p => p.Product_id == id);
            return View(products.ToList());
        }

        public ActionResult Add_ToCart(int id,string quantity)
        {
            cart Cart = new cart();
            Cart.Product_id = id;
          
                Cart.Quantity = 1;
            
            Cart.userId = User.Identity.GetUserId();
            db.carts.Add(Cart);
            db.SaveChanges();
            var userId = User.Identity.GetUserId();

            Session["NumberofItem"] = db.carts.Where(user => user.userId == userId).Count();
            return RedirectToAction("ViewAdd_ToCart", "Products");
        }
      
        public ActionResult Remove_FromCard(int id)
        {

            cart Cart = db.carts.Find(id);
            db.carts.Remove(Cart);
            db.SaveChanges();
            return RedirectToAction("ViewAdd_ToCart", "Products");
        }
        [HttpPost]
        public ActionResult UpdateCart(FormCollection form)
        {
            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("quantity-"))
                {
                    int cartItemId = int.Parse(key.Replace("quantity-", ""));
                    int quantity = int.Parse(form[key]);
                    var cartItem = db.carts.Find(cartItemId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = quantity;
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("ViewAdd_ToCart");
        }
       



        public ActionResult ViewAdd_ToCart()
        {


            var userId = User.Identity.GetUserId();
            var user_Cart = db.carts.Where(user => user.userId == userId);
            float TotalPrice = 0;
            foreach(var item in user_Cart)
            {
                float productPrice =float.Parse(Convert.ToString(item.Product.Product_Price));
                int NumberOfItems = int.Parse(Convert.ToString(item.Quantity));
                TotalPrice += productPrice * NumberOfItems;

            }
            ViewBag.TotalPrice = TotalPrice;    
            return View(user_Cart.ToList());
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_id,Product_Name,Product_Image,Product_Price,Product_Size,Product_Color,Quantity,Product_Description,Category_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", product.Subcategory_id);
            return View(product);
        }

        // GET: Products/Edit/5
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
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", product.Subcategory_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_id,Product_Name,Product_Image,Product_Price,Product_Size,Product_Color,Quantity,Product_Description,Category_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Category_id", "Category_Name", product.Subcategory_id);
            return View(product);
        }

        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
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
