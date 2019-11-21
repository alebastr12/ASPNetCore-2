using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Areas.Admin.Models;
using WebStore.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.Domain.Entitys;
using WebStore.Domain.Filters;
using WebStore.Domain.EntitysDTO;
using WebStore.Services.Map;
using Microsoft.Extensions.Logging;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly List<SelectListItem> brandList= new List<SelectListItem>();
        private readonly List<SelectListItem> categoryList = new List<SelectListItem>();
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ILogger<HomeController> log;

        public HomeController(IProductService productService, IHostingEnvironment appEnvironment, ILogger<HomeController> log)
        {
            this._appEnvironment = appEnvironment;
            this.log = log;
            this.productService = productService;
            brandList.Add(new SelectListItem { Text = "", Value = "" });
            categoryList.Add(new SelectListItem { Text = "", Value = "" });
            brandList.AddRange(productService.GetBrands()
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList());
            categoryList.AddRange(productService.GetCategories()
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList());
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductList(ProductFilter filter)
        {
            //ViewBag.Brands = brandList;
            //ViewBag.Category = categoryList;
            var products = new AdminProductViewModel
            {
                ProductList = productService.GetProducts(filter).Select(ProductMapper.FromDTO),
                BrandList = brandList,
                CategoryList = categoryList
            };
            return View(products);
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.Brands = brandList;
            ViewBag.Category = categoryList;
            if (!id.HasValue)
            {
                return View(new EditProductViewModel());
            }
            var editItem = productService.GetProductById(id.Value);
            if (editItem is null)
                return NotFound();
            
            return View(new EditProductViewModel 
            { 
                Id=editItem.Id,
                BrandId=editItem.Brand.Id,
                CategoryId=editItem.Category.Id,
                Name=editItem.Name,
                Order=editItem.Order,
                Price=editItem.Price
                
            });
        }
        public IActionResult Delete(int id)
        {
            log.LogInformation($"Запрос на удаление продукта с id {id}");
            productService.Delete(id);
            //productService.Commit();
            return RedirectToAction(nameof(ProductList));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            log.LogInformation("Редактирование продукта или добавление нового.");
            if (model.BrandId == 0)
            {
                ModelState.AddModelError("BrandId", "Нужно выбрать брэнд");
            }
            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Нужно выбрать категорию");
            }
            if (model.Price == 0)
            {
                ModelState.AddModelError("Price", "Это слишком дешево!");
            }
            if (!ModelState.IsValid)
            {
                log.LogError($"При запросе на редактирование/добавление продукта передана не валидная модель.");
                ViewBag.Brands = brandList;
                ViewBag.Category = categoryList;
                return View(model);
            }
            log.LogInformation("Модель для редактирования прошла валидацию.");
            if (model.ImageUrl != null)
            {
                string path = "/images/shop/" + model.ImageUrl.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.ImageUrl.CopyToAsync(fileStream);
                }
                log.LogInformation($"Изображение товара сохранено на сервер {path}");
                //FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                //_context.Files.Add(file);
                //_context.SaveChanges();
            }
            if (model.Id > 0)
            {
                var editProd = productService.GetProductById(model.Id);
                if (editProd is null)
                {
                    log.LogError($"Продукт с идентификатором {model.Id} не найден в БД. Редактирование отменено.");
                    return NotFound();
                }
                log.LogInformation($"Продукт с идентификатором {model.Id} найден в БД. Отправлен запрос на редактирование.");
                productService.UpdateProduct(new ProductDTO
                {
                    Id = model.Id,
                    Brand = new BrandDTO { Id = model.BrandId },
                    Category = new CategoryDTO { Id = model.CategoryId },
                    ImageUrl = model.ImageUrl?.FileName??editProd.ImageUrl,
                    Name = model.Name,
                    Order = model.Order,
                    Price = model.Price
                });
            }
            else
            {
                log.LogInformation($"Запрос на добавление нового продукта с именем {model.Name}");
                productService.AddProduct(new ProductDTO
                {
                    Brand=new BrandDTO { Id= model.BrandId },
                    Category=new CategoryDTO { Id= model.CategoryId },
                    ImageUrl=model.ImageUrl?.FileName,
                    Name=model.Name,
                    Order=model.Order,
                    Price=model.Price
                });
            }
            //productService.Commit();
            return RedirectToAction("ProductList");
        }
    }
}
