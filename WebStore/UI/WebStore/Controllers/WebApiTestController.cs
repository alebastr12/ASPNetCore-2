using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValueService valueService;

        public WebApiTestController(IValueService valueService)
        {
            this.valueService = valueService;
        }
        public async Task<IActionResult> Index()
        {
            var value = await valueService.GetAsync();
            return View(value);
        }
    }
}