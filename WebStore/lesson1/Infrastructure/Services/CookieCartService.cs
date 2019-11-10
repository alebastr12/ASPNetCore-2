using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Filters;

namespace WebStore.Infrastructure.Services
{
    public class CookieCartService : ICartService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IProductService productService;
        private readonly string cartName;

        private Cart Cart
        {
            get
            {
                var cookie = contextAccessor
                                .HttpContext
                                .Request
                                .Cookies[cartName];
                string json = string.Empty;
                Cart cart = null;

                if (cookie == null)
                {
                    cart = new Cart { Items = new List<CartItem>() };
                    json = JsonConvert.SerializeObject(cart);

                    contextAccessor
                          .HttpContext
                          .Response
                          .Cookies
                          .Append(
                             cartName,
                             json,
                             new CookieOptions
                             {
                                 Expires = DateTime.Now.AddDays(1)
                             });
                    return cart;
                }

                json = cookie;
                cart = JsonConvert.DeserializeObject<Cart>(json);

                contextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Delete(cartName);

                contextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Append(
                          cartName,
                          json,
                          new CookieOptions()
                          {
                              Expires = DateTime.Now.AddDays(1)
                          });

                return cart;
            }

            set
            {
                var json = JsonConvert.SerializeObject(value);

                contextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Delete(cartName);
                contextAccessor
                      .HttpContext
                      .Response
                      .Cookies
                      .Append(
                           cartName,
                           json,
                           new CookieOptions()
                           {
                               Expires = DateTime.Now.AddDays(1)
                           });
            }
        }
        public CookieCartService(IHttpContextAccessor contextAccessor, IProductService productService)
        {
            this.contextAccessor = contextAccessor;
            this.productService = productService;
            cartName = contextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? contextAccessor.HttpContext.User.Identity.Name : "";
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
            Cart = new Cart { Items = new List<CartItem>() };
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
