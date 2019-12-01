using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService cartService;

        public CartViewComponent(ICartService cartService)
        {
            this.cartService = cartService;
        }
        public IViewComponentResult Invoke() => View(cartService.TransformCart());
    }
}
