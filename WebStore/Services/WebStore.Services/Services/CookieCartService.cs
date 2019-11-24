using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Filters;

namespace WebStore.Services.Services
{
    public class CookieCartService : ICartService
    {
        
        private readonly IProductService productService;
        private readonly ICartStore cartStore;
        private readonly string cartName;

        private Cart Cart
        {
            get => cartStore.Cart; set => cartStore.Cart = value;
        }
        public CookieCartService(IProductService ProductData, ICartStore CartStore)
        {
            productService = ProductData;
            cartStore = CartStore;
        }
        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;
            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item != null)
            {
                if (item.Quantity > 0)
                    item.Quantity--;
                if (item.Quantity==0)
                    cart.Items.Remove(item);
            }
            Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item!=null)
                cart.Items.Remove(item);
            Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = productService.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(c => c.ProductId).ToList()
            }).Select(p => new ProductViewModel
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty
            }).ToList();
            var r = new CartViewModel
            {
                Items = Cart.Items.ToDictionary(x => products.First(y => y.Id == x.ProductId),
                x => x.Quantity)
            };
            return r;
        }
    }
}
