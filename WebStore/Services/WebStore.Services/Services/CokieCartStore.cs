using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
namespace WebStore.Services.Services
{
    public class CokieCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        public Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cookie = context.Request.Cookies[_CartName];
                if (cookie is null)
                {
                    var cart = new Cart();
                    cart.Items = new List<CartItem>();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookie(cookies, cookie);
                return JsonConvert.DeserializeObject<Cart>(cookie);
            }
            set => ReplaceCookie(_HttpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(1) });
        }

        public CokieCartStore(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;

            var user = HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _CartName = $"cart{user_name}";
        }
    }

}
