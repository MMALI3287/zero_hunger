using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zero_hunger.EF;
using zero_hunger.Models;

namespace zero_hunger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login model)
        {
            var db = new zero_hungerEntities2();
            var extUser=(from u in db.Registrations where u.username==model.username && u.password==model.password select u).SingleOrDefault();
            if (extUser != null)
            {
                Session["Rid"]=extUser.id;
                if (extUser.user_type.Equals("Admin"))
                {
                    return RedirectToAction("Index","Admin");
                }
                if (extUser.user_type.Equals("Employees"))
                {
                    return RedirectToAction("Index","Employees");
                }
                if (extUser.user_type.Equals("Restaurents"))
                {
                    return RedirectToAction("Index","Restaurents");
                }
            }
            else
            {
                ViewBag.msg = "Logn Failed";
            }
            return View(); 
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            return View();  
        }

        [HttpPost]
        public ActionResult SignUp(RegistrationClass model)
        {
            var db = new zero_hungerEntities2();
            var user = new Registration()
            {
                username = model.username, password = model.password, user_type = model.user_type
            };
            db.Registrations.Add(user);
            db.SaveChanges();
            var rid=(from r in db.Registrations where r.username==model.username select r).SingleOrDefault();
            if (model.user_type.Equals("Employees"))
            {
                var employee = new Employee()
                {
                    name= model.name,phone=model.phone,email=model.email,Rid=rid.id
                };
                db.Employees.Add(employee); 
            }
            if (model.user_type.Equals("Restaurents"))
            {
                var restaurents = new Restaurant()
                {
                    supplier_name = model.name,
                    contact_number = model.phone,
                    name="-",location="-",
                    Rid = rid.id
                };
                db.Restaurants.Add(restaurents);
            }
            if (model.user_type.Equals("Admin"))
            {
                var admin = new Admin()
                {
                    name = model.name,
                    phone = model.phone,
                  email=model.email,
                    Rid = rid.id
                };
                db.Admins.Add(admin);
            }
            db.SaveChanges();
            ViewBag.msg = "Registration Successfull";
            return View();
        }

    }
}