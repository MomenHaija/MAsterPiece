using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Plant_Paradise.Models;
using static System.Collections.Specialized.BitVector32;

namespace Plant_Paradise.Controllers
{
    public class TransactionsController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: Transactions
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user_Cart = db.carts.Where(user => user.userId == userId);
            float TotalPrice = 0;
            foreach (var item in user_Cart)
            {
                float productPrice = float.Parse(Convert.ToString(item.Product.Product_Price));
                int NumberOfItems = int.Parse(Convert.ToString(item.Quantity));
                TotalPrice += productPrice * NumberOfItems;

            }
            ViewBag.TotalPrice = TotalPrice;
            return View(user_Cart.ToList());
        }

        public ActionResult MakeOrder()
        {
            Session["USerid"] =User.Identity.GetUserId();

            var userId = User.Identity.GetUserId();
            var cartItems = db.carts.Where(c => c.userId == userId).ToList();

            Order order = new Order
            {
                Order_id =Convert.ToString(Guid.NewGuid()),
                Order_date = DateTime.Now,
                userId = userId,

            };
            db.Orders.Add(order);
            db.SaveChanges();
            float TotalPrice = 0;
            foreach (var cartItem in cartItems)
                {
                var itemprice = cartItem.Product.Product_Price;
                var itemQuentity = cartItem.Quantity;
                TotalPrice +=float.Parse(Convert.ToString(itemprice * itemQuentity));
                Order_Details orderDetail = new Order_Details
                {

                        Order_id = order.Order_id,
                        Product_id = cartItem.Product_id,
                        Quantity = cartItem.Quantity,
                };
                    // Associate the OrderDetail objects with the Order object
                    db.Order_Details.Add(orderDetail);
                    // Remove the cart item from the Cart table
                    db.carts.Remove(cartItem);

                }
                Session["NumberofItem"] = 0;
                order.Total_price = TotalPrice;
                db.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            

        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Order_id = new SelectList(db.Orders, "Order_id", "userId");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,Amount,TransactionDate,Order_id,userId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", transaction.userId);
            ViewBag.Order_id = new SelectList(db.Orders, "Order_id", "userId", transaction.Order_id);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", transaction.userId);
            ViewBag.Order_id = new SelectList(db.Orders, "Order_id", "userId", transaction.Order_id);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,Amount,TransactionDate,Order_id,userId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", transaction.userId);
            ViewBag.Order_id = new SelectList(db.Orders, "Order_id", "userId", transaction.Order_id);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
