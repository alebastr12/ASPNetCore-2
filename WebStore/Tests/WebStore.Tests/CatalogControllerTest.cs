
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Filters;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests
{
    [TestClass]
    public class CatalogControllerTest
    {
        [TestMethod]
        public void ProductDetail_Correct_Return_Item()
        {
            const int expected_id = 1;
            const decimal expected_price = 120m;
            string expected_name = $"product {expected_id}";
            string expected_brand_name = $"brand {expected_id}";
            string expected_category_name = $"category {expected_id}";
            string expected_image = $"image of {expected_id} product";
            var config_data_mock = new Mock<IConfiguration>();
            var product_data_mock = new Mock<IProductService>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDTO
                {
                    Id=expected_id,
                    Brand = new BrandDTO { Id=1, Name=expected_brand_name, Order=0},
                    Category = new CategoryDTO { Id=1, Name=expected_category_name, Order=0},
                    ImageUrl= expected_image,
                    Name=expected_name,
                    Order=0,
                    Price=expected_price
                });
            var controller = new CatalogController(product_data_mock.Object, config_data_mock.Object);

            var result = controller.ProductDetails(expected_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);
            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_image, model.ImageUrl);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.BrandName);
        }
        [TestMethod]
        public void ProductDetails_Returns_NotFound()
        {
            var config_data_mock = new Mock<IConfiguration>();
            var product_data_mock = new Mock<IProductService>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id=>default);

            var controller = new CatalogController(product_data_mock.Object,config_data_mock.Object);

            var result = controller.ProductDetails(1);

            Assert.IsType<NotFoundResult>(result);
        }
        [TestMethod]
        public void Shop_Return_Correct_View()
        {
            const int expected_count_product = 10;
            const int expected_category_id = 5;
            const int expected_brand_id = 5;
            var product_data_mock = new Mock<IProductService>();
            product_data_mock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter =>
                    new PagedProductDTO
                    {
                        Products = Enumerable.Range(0, expected_count_product).Select(i =>
                            new ProductDTO
                            {
                                Id = i,
                                Brand = new BrandDTO { Id = 1, Name = $"BrandName_{i}", Order = 0 },
                                Category = new CategoryDTO { Id = 1, Name = $"CategoryName_{i}", Order = 0 },
                                ImageUrl = $"Image_{i}",
                                Name = $"Product_{i}",
                                Order = i,
                                Price = 100 * i
                            }),
                        TotalCount = expected_count_product
                    });
            var config_data_mock = new Mock<IConfiguration>();

            var controller = new CatalogController(product_data_mock.Object,config_data_mock.Object);

            var result = controller.Shop(expected_category_id, expected_brand_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.Model);

            Assert.Equal(expected_count_product, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_category_id, model.CategoryId);

            Assert.Equal("Product_0", model.Products.ElementAt(0).Name);
            Assert.Equal("Image_5", model.Products.ElementAt(5).ImageUrl);
            Assert.Equal("BrandName_7", model.Products.ElementAt(7).BrandName);
            Assert.Equal(300, model.Products.ElementAt(3).Price);
        }
    }
}
