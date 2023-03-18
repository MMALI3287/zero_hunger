using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.EF;

namespace zero_hunger.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageEmployees()
        {
            var db = new zero_hungerEntities2();
            var extemps = db.Employees;
            return View(extemps);
        }
        public ActionResult ManageRestaurents()
        {
            var db = new zero_hungerEntities2();
            var extrest = db.Restaurants;
            return View(extrest);
        }

        public ActionResult ManageRequests()
        {
            var db = new zero_hungerEntities2();
            var extreqst = db.CollectRequests;
            return View(extreqst);
        }
        [HttpGet]
        public ActionResult EmpEdit(int id) { 
        
            var db = new zero_hungerEntities2();    
            var extemps = db.Employees.Find(id);
            return View(extemps);  
        }
        [HttpPost]
        public ActionResult EmpEdit(Employee model)
        {
            var db = new zero_hungerEntities2();
            var extemps = db.Employees.Find(model.id);
            extemps.email = model.email;
            extemps.phone = model.phone;
            extemps.name = model.name;
            db.SaveChanges();
            ViewBag.msg = "Successfully saved";
            return View();
        }
        public ActionResult EmpDelete(int id) {
            var db = new zero_hungerEntities2();
            var extemp=  db.Employees.Find(id);
            var extreg = (from r in db.Registrations where r.id == extemp.Rid select r).SingleOrDefault();
            var extreqst=(from cr in db.CollectRequests where cr.collection_employee_id == id select cr).ToList();
            db.Registrations.Remove(extreg);
            db.SaveChanges();
            foreach (var r in extreqst)
            {
                r.collection_employee_id = null;
            }
            db.Employees.Remove(extemp);
            db.SaveChanges();       
        return RedirectToAction("ManageEmployees");
        }

        public ActionResult pending(int id)
        {
            var db = new zero_hungerEntities2();
            var extreqst= db.CollectRequests.Find(id);
            extreqst.collection_status = "Pending";
            db.SaveChanges();
            return RedirectToAction("ManageRequests");
        }
        [HttpGet]
        public ActionResult ResEdit(int id)
        {
            var db = new zero_hungerEntities2();    
            var extrest= db.Restaurants.Find(id);
            return View(extrest);
        }
        [HttpPost]
        public ActionResult ResEdit(Restaurant model)
        {
            var db = new zero_hungerEntities2();
            var extrest = db.Restaurants.Find(model.id);
            extrest.contact_number = model.contact_number;
            extrest.supplier_name = model.supplier_name;    
            extrest.name = model.name;  
            extrest.location=model.location;
            ViewBag.msg = "Saved Successfully";
            db.SaveChanges();   
            return View();
        }

        public ActionResult ResDelete(int id)
        {
            var db = new zero_hungerEntities2();
            var extres = db.Restaurants.Find(id);
            var extreg = (from r in db.Registrations where r.id == extres.Rid select r).SingleOrDefault();
            var extreqst = (from cr in db.CollectRequests where cr.restaurant_id == id select cr).ToList();
            db.Registrations.Remove(extreg);
            db.SaveChanges();
            foreach (var r in extreqst)
            {
                if (r.collection_status.Equals("Pending"))
                {
                    db.CollectRequests.Remove(r);
                }
                else
                {
                    r.restaurant_id = null;
                }  
            }
            db.Restaurants.Remove(extres);
            db.SaveChanges();
            return RedirectToAction("ManageRestaurents");
        }


        public ActionResult ReqDelete(int id)
        {
            return View();  
        }

        [HttpGet]
        public ActionResult ReqEdit(int id)
        {
            var db = new zero_hungerEntities2 ();
            var extreq= db.CollectRequests.Find(id);
            return View(extreq);
        }
        [HttpPost]
        public ActionResult ReqEdit(CollectRequest model)
        {
            var db = new zero_hungerEntities2();
            var extreq = db.CollectRequests.Find(model.id);
            extreq.max_preservation_time = model.max_preservation_time;
            extreq.quantity = model.quantity;
            extreq.collection_status = model.collection_status;
            extreq.food_type = model.food_type;
            
            db.SaveChanges ();
            return RedirectToAction("ManageRequests");
        }

    }
}