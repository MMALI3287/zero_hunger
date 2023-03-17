using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.EF;

namespace zero_hunger.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewRequests()
        {
            var db = new zero_hungerEntities2();
            var extRequest = (from cr in db.CollectRequests where cr.collection_status=="Pending" select cr).ToList();
            return View(extRequest);
        }
        public ActionResult AcceptedRequests()
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extRequest = (from cr in db.CollectRequests where cr.collection_status != "Pending" && cr.Employee.Rid == eid  select cr).ToList();
            return View(extRequest);
        }

        public ActionResult Accept(int id)
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extemp = (from r in db.Employees where r.Rid == eid select r).SingleOrDefault();
            var extrequest = db.CollectRequests.Find(id);
            extrequest.collection_status = "Accepted";
            extrequest.collection_employee_id = extemp.id;
            db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
        public ActionResult Cancel(int id)
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrequest = db.CollectRequests.Find(id);
            extrequest.collection_status = "Pending";
            extrequest.collection_employee_id = null;
            db.SaveChanges();
            return RedirectToAction("AcceptedRequests");
        }

        public ActionResult Collected(int id)
        {
            var db = new zero_hungerEntities2();
            var extrequest = db.CollectRequests.Find(id);
            extrequest.collection_status = "Collected";
            extrequest.collection_time = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("AcceptedRequests");
        }

        public ActionResult Delivered(int id)
        {
            var db = new zero_hungerEntities2();
            var extrequest = db.CollectRequests.Find(id);
            extrequest.collection_status = "Delivered";
            extrequest.collection_time = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("AcceptedRequests");
        }
        public ActionResult Profile()
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extemp = (from r in db.Employees where r.Rid == eid select r).SingleOrDefault();
            return View(extemp);
        }
        [HttpGet]
        public ActionResult PEdit()
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extemp= (from r in db.Employees where r.Rid == eid select r).SingleOrDefault();
            return View(extemp);
        }
        [HttpPost]
        public ActionResult PEdit(Employee model)
        {
            int eid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extemp = (from r in db.Employees where r.Rid == eid select r).SingleOrDefault();
             extemp.name= model.name;
            extemp.phone = model.phone;
            extemp.email = model.email;
            db.SaveChanges();
            ViewBag.msg = "Successfully Saved";
            return View();
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }
    }
}