using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Plant_Paradise.Models;

namespace Plant_Paradise.Controllers
{
    public class UserProfileController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: UserProfile
        public ActionResult Index()
        {
            var User_id = User.Identity.GetUserId();
            return View(db.AspNetUsers.Where(p=>p.Id== User_id).ToList());
        }
        public ActionResult EditProfile([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Phone_Number,User_Address,Full_Name,User_Image")] AspNetUser aspNetUser ,string name,string email,string address,string phone, HttpPostedFileBase user_image)
        {

            string fileName = Path.GetFileName(user_image.FileName);
            string path = "../Images/" + fileName;
            string path2 = Path.GetFileName(user_image.FileName);
            string fullPath = Server.MapPath(path);
            user_image.SaveAs(fullPath);


            var User_id = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(p => p.Id == User_id).FirstOrDefault();
            user.User_Image=path2.ToString();   
            user.Full_Name =name;
            user.Email = email;
            user.Phone_Number = phone;
            user.User_Address = address;
            
            db.SaveChanges();
            return RedirectToAction("Index", "UserProfile");
        }
        public PartialViewResult Oreder_User()
        {
            var User_id = User.Identity.GetUserId();
            var order = db.Orders.Where(p => p.userId == User_id);
            return PartialView("UserOrder", order.ToList());
        }
        
        public PartialViewResult ProductsinOrders(string id)
        {
            var order_detailes = db.Order_Details.Where(p => p.Order_id== id);
            return PartialView("ProductsinOrders", order_detailes.ToList());
        }

        // GET: UserProfile/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: UserProfile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Phone_Number,User_Address,Full_Name")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: UserProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Phone_Number,User_Address,Full_Name")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: UserProfile/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: UserProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
