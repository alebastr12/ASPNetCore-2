using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogSingle()
        {
            return View();
        }
        //public IActionResult Cart()
        //{
        //    return View();
        //}
        //public IActionResult Checkout()
        //{
        //    return View();
        //}
        public IActionResult Contact()
        {
            return View();
        }
        //public IActionResult Login()
        //{
        //    return View();
        //}

        public IActionResult Error404()
        {
            return View();
        }
        public IActionResult ErrorStatus(string id)
        {
            switch (id)
            {
                case "404": return RedirectToAction(nameof(Error404));
                default: return Content($"Статусный код ошибки {id}");
            }
        }
    }
}