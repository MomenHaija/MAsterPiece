using Microsoft.AspNet.Identity;
using Plant_Paradise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plant_Paradise.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        PlantsEntities db = new PlantsEntities();

        public ActionResult Index()
        {
            var users = db.AspNetUsers;
            return View(users.ToList());
        }
        [HttpPost]
        public ActionResult search(string Search)
        {
            var users = db.AspNetUsers.Where(p=>p.Full_Name.Contains(Search) || p.Email.Contains(Search) || p.User_Address.Contains(Search)); 
            return View("Index", users.ToList());
        }
        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
