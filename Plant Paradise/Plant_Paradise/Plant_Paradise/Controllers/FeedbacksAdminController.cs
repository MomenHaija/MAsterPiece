using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plant_Paradise.Models;

namespace Plant_Paradise.Controllers
{
    public class FeedbacksAdminController : Controller
    {
        private PlantsEntities db = new PlantsEntities();

        // GET: FeedbacksAdmin
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.AspNetUser).Where(p=>p.IsAccepted==0);
            return View(feedbacks.ToList());
        }
        [HttpPost]
        public ActionResult search(string Search)
        {
            var feedbacks = db.Feedbacks.Include(f => f.AspNetUser).Where(user=>user.AspNetUser.Full_Name.Contains(Search) || user.AspNetUser.Email.Contains(Search) || user.Feedback_text.Contains(Search)).Where(p => p.IsAccepted == 0);

            return View("Index", feedbacks.ToList());
        }
        public ActionResult Accept(int id)
        {


            var feedback = db.Feedbacks.Where(f => f.Feedback_id == id).FirstOrDefault();
            feedback.IsAccepted = 1;
            db.SaveChanges();
            return RedirectToAction("Index", "FeedbacksAdmin");


        }
        public ActionResult Reject(int id)
        {


            var feedback = db.Feedbacks.Where(f => f.Feedback_id == id).FirstOrDefault();
            feedback.IsAccepted = 2;
            db.SaveChanges();
            return RedirectToAction("Index", "FeedbacksAdmin");


        }
        // GET: FeedbacksAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: FeedbacksAdmin/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: FeedbacksAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Feedback_id,Feedback_text,Feedback_Date,userId,IsAccepted")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", feedback.userId);
            return View(feedback);
        }

        // GET: FeedbacksAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", feedback.userId);
            return View(feedback);
        }

        // POST: FeedbacksAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Feedback_id,Feedback_text,Feedback_Date,userId,IsAccepted")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", feedback.userId);
            return View(feedback);
        }

        // GET: FeedbacksAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: FeedbacksAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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
