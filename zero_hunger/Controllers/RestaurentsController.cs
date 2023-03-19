using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using zero_hunger.EF;

namespace zero_hunger.Controllers
{
    public class RestaurentsController : Controller
    {
        // GET: Restaurents
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddRequest() 
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            if(extrestaurent.name.Equals("-") || extrestaurent.location.Equals("-"))
            {
                TempData["addName"] = "Please Add Restaurent Name and Location";
                Session["Redirect"] = "AddRequest";
                return RedirectToAction("PEdit");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddRequest(CollectRequest model) 
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            var newRequest = new CollectRequest()
            {
                restaurant_id = extrestaurent.id,
                food_type = model.food_type,
                quantity = model.quantity,
                max_preservation_time = model.max_preservation_time,
                collection_status = "Pending",
                collection_time = null,
            };
            db.CollectRequests.Add(newRequest);
            db.SaveChanges();
            ViewBag.msg = "Request Succefully Sent";
            return View();
        }
        public ActionResult MyRequests()
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            var extrequest = (from cr in db.CollectRequests where cr.restaurant_id == extrestaurent.id select cr).ToList();
            return View(extrequest);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new zero_hungerEntities2();
            var extCollecReq = db.CollectRequests.Find(id);
            return View(extCollecReq);
        }
        [HttpPost]
        public ActionResult Edit(CollectRequest model)
        {

            var db = new zero_hungerEntities2();
            var extCollecReq = db.CollectRequests.Find(model.id);
            extCollecReq.quantity = model.quantity;
            extCollecReq.food_type = model.food_type;
            extCollecReq.max_preservation_time = model.max_preservation_time;
            db.SaveChanges();
            ViewBag.msg = "Successfully Saved Changes!";
            return View();
        }
      
        public ActionResult Profile()
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            return View(extrestaurent);
        }
        [HttpGet]
        public ActionResult PEdit()
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            return View(extrestaurent);
        }
        [HttpPost]

        public ActionResult PEdit(Restaurant model)
        {
            int id = (int)Session["Rid"];
            var db = new zero_hungerEntities2();
            var extrestaurent = (from r in db.Restaurants where r.Rid == id select r).SingleOrDefault();
            extrestaurent.name = model.name;
            extrestaurent.location = model.location;
            extrestaurent.contact_number = model.contact_number;
            extrestaurent.supplier_name = model.supplier_name;
            db.SaveChanges();
            ViewBag.msg = "Successfully Saved";
            if (!Session["Redirect"].Equals(null))
            {
                Session["Redirect"] = null;
                return RedirectToAction("AddRequest");
            }
            else
            return View();
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Login","Home");
        }

        public ActionResult Delete(int id)
        {
            var db = new zero_hungerEntities2();
            var extrequest = db.CollectRequests.Find(id);
            db.CollectRequests.Remove(extrequest);
            db.SaveChanges();
            return RedirectToAction("MyRequests");
        }

    }
}
