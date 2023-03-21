using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
                    Session["Redirect"] = "";
                    return RedirectToAction("Index","Restaurents");
                }
            }
            else
            {
                ViewBag.msg = "Login Failed";
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
            var extUsername = (from u in db.Registrations where u.username == model.username select u).SingleOrDefault();
            if(extUsername == null)
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
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("mahtab.sani12381@gmail.com");
                    mail.To.Add(new MailAddress(model.email));
                    mail.Subject = "Account Created";
                    mail.Body = "<html><body><h1>Welcome to Zero Hunger Community</h1><br>Congratulation Your Account created Successfully</body</html>";
                    mail.IsBodyHtml = true;
                    

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587; // or 465 for SSL
                    smtp.EnableSsl = true; // set to true for SSL encryption
                    smtp.Credentials = new NetworkCredential("mahtab.sani12381@gmail.com", "ozvxmghbzclxkvnx"); // set the username and password for the SMTP server
                    smtp.Send(mail);
                }
                catch
                {
                   
                }
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