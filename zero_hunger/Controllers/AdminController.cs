using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.Auth;
using zero_hunger.EF;
using zero_hunger.Models;

namespace zero_hunger.Controllers
{
    [AdminLogged]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var db = new zero_hungerEntities2();
            var emp = db.Employees.ToList();
            var rest = db.Restaurants.ToList();
            var admin = db.Admins.ToList();
            var pending = (from p in db.CollectRequests where p.collection_status=="Pending" select p).ToList();
            var collected = (from p in db.CollectRequests where p.collection_status == "Collected" select p).ToList();
            var delivered = (from p in db.CollectRequests where p.collection_status == "Delivered" select p).ToList();
            int admincount=admin.Count;
            int empcount = emp.Count;
            int rescount = rest.Count;
            int pendingcount=pending.Count;
            int collectedcount=collected.Count;
            int deliveredcount=delivered.Count;
            ViewBag.empcount=empcount;
            ViewBag.admincount=admincount;
            ViewBag.rescount=rescount;
            ViewBag.pendingcount=pendingcount;
            ViewBag.deliveredcount=deliveredcount;  
            ViewBag.collectedcount=collectedcount;
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
            var db = new zero_hungerEntities2();
            var extreq = db.CollectRequests.Find(id);
            db.CollectRequests.Remove(extreq);
            db.SaveChanges();
            return RedirectToAction("ManageRequests");
        }

        [HttpGet]
        public ActionResult ReqEdit(int id)
        {
            var db = new zero_hungerEntities2 ();
            var extreq= db.CollectRequests.Find(id);
            var extemp = db.Employees.ToList();
            ViewBag.data = extemp;
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
            if (!model.collection_status.Equals("Pending"))
            {
                extreq.collection_employee_id = model.collection_employee_id;
                extreq.collection_time = model.collection_time;
            }
            else
            {
                extreq.collection_employee_id = null;
                extreq.collection_time = null;
            }
            db.SaveChanges ();
            return RedirectToAction("ManageRequests");
        }

        public new ActionResult Profile()
        {
            int aid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extadmin = (from r in db.Admins where r.Rid == aid select r).SingleOrDefault();
            return View(extadmin);
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult PEdit()
        {
            int aid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extadmin = (from r in db.Admins where r.Rid == aid select r).SingleOrDefault();
            return View(extadmin);
        }
        [HttpPost]
        public ActionResult PEdit(Employee model)
        {
            int aid = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extadmin = (from r in db.Admins where r.Rid == aid select r).SingleOrDefault();
            extadmin.name = model.name;
            extadmin.phone = model.phone;
            extadmin.email = model.email;
            db.SaveChanges();
            ViewBag.msg = "Successfully Saved";
            return View();
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(RegistrationClass model)
        {
            var db = new zero_hungerEntities2();
            var extUsername = (from u in db.Registrations where u.username == model.username select u).SingleOrDefault();
            if (extUsername == null)
            {
                var user = new Registration()
                {
                    username = model.username,
                    password = model.password,
                    user_type = model.user_type
                };
                db.Registrations.Add(user);
                db.SaveChanges();
                var rid = (from r in db.Registrations where r.username == model.username select r).SingleOrDefault();
                if (model.user_type.Equals("Employees"))
                {
                    var employee = new Employee()
                    {
                        name = model.name,
                        phone = model.phone,
                        email = model.email,
                        Rid = rid.id
                    };
                    db.Employees.Add(employee);
                }
                if (model.user_type.Equals("Admin"))
                {
                    var admin = new Admin()
                    {
                        name = model.name,
                        phone = model.phone,
                        email = model.email,
                        Rid = rid.id
                    };
                    db.Admins.Add(admin);
                }
                if (model.user_type.Equals("Restaurents"))
                {
                    var restaurents = new Restaurant()
                    {
                        supplier_name = model.name,
                        contact_number = model.phone,
                        name = "-",
                        location = "-",
                        Rid = rid.id
                    };
                    db.Restaurants.Add(restaurents);
                }
                db.SaveChanges();
                ViewBag.msg = "Registration Successfull";
                return View();
            }
            else
            {
                ViewBag.msg = "User Name exists";
                return View();
            }
        }

    }
}