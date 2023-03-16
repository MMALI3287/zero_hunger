using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zero_hunger.Controllers
{
    public class RestaurentsController : Controller
    {
        // GET: Restaurents
        public ActionResult Index()
        {
            return View();
        }
    }
}