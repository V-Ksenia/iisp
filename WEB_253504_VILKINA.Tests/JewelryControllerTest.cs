using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI.Controllers;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.Tests
{
    public class JewelryControllerTest
    {
        private readonly ICategoryService _categoryService;
        private readonly IJewelryService _jewelryService;
        private readonly ProductController _productController;

        public JewelryControllerTest()
        {
            _categoryService = Substitute.For<ICategoryService>();
            _jewelryService = Substitute.For<IJewelryService>();
            _productController = new ProductController(_jewelryService, _categoryService);
        }

        [Fact]
        public async Task Index_Returns404_WhenCategoryListFails()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = false,
                ErrorMessage = "Error 404"
            }));

            _jewelryService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ProductListModel<Jewelry>>
            {
                Successfull = true,
                Data = new ProductListModel<Jewelry> { Items = new List<Jewelry>() }
            }));

            // Act
            var result = await _productController.Index(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error 404", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_Returns404_WhenJewelryListFails()
        {
            // Arrange
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = new List<Category> { new Category { Name = "Category1" } }
            }));

            _jewelryService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ProductListModel<Jewelry>>
            {
                Successfull = false,
                ErrorMessage = "Error 404"
            }));

            // Act
            var result = await _productController.Index(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error 404", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_PopulatesViewDataWithCategories_WhenSuccessful()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)"");

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _productController.TempData = tempDataDictionary;

            _productController.ControllerContext = controllerContext;

            var categories = new List<Category> { new Category { Name = "Category1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = categories
            }));
            _jewelryService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
                .Returns(Task.FromResult(new ResponseData<ProductListModel<Jewelry>>
                {
                    Successfull = true,
                    Data = new ProductListModel<Jewelry>
                    {
                        Items = new List<Jewelry> { new Jewelry { Description = "Jewelry1" } }
                    }
                }));

            // Act
            var result = await _productController.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualCategories = viewResult.ViewData["categories"] as List<Category>;
            Assert.NotNull(actualCategories);
            Assert.Equal(categories.Count, actualCategories.Count);
            Assert.Equal(categories[0].Name, actualCategories[0].Name);
        }


        [Theory]
        [InlineData(null, "All")]
        [InlineData("category1", "Category1")]
        public async Task Index_SetsCorrectCurrentCategory(string category, string expectedCurrentCategory)
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)"");

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _productController.TempData = tempDataDictionary;

            _productController.ControllerContext = controllerContext;

            var categories = new List<Category>
    {
        new Category { Name = "Category1", NormalizedName = "category1" }
    };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = categories
            }));
            _jewelryService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ProductListModel<Jewelry>>
            {
                Successfull = true,
                Data = new ProductListModel<Jewelry>
                {
                    Items = new List<Jewelry> { new Jewelry { Description = "Jewelry1" } }
                }
            }));

            // Act
            var result = await _productController.Index(category);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedCurrentCategory, viewResult.ViewData["currentCategory"]);
        }

        [Fact]
        public async Task Index_PassesJewelryListToViewModel_WhenSuccessful()
        {
            // Arrange
            var request = Substitute.For<HttpRequest>();
            request.Headers["X-Requested-With"].Returns((Microsoft.Extensions.Primitives.StringValues)"");

            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(request);

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
            _productController.TempData = tempDataDictionary;

            _productController.ControllerContext = controllerContext;

            var jewelries = new List<Jewelry> { new Jewelry { Description = "Jewelry1" } };
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(new ResponseData<List<Category>>
            {
                Successfull = true,
                Data = new List<Category> { new Category { Name = "Category1" } }
            }));
            _jewelryService.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(Task.FromResult(new ResponseData<ProductListModel<Jewelry>>
            {
                Successfull = true,
                Data = new ProductListModel<Jewelry> { Items = jewelries }
            }));

            // Act
            var result = await _productController.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductListModel<Jewelry>>(viewResult.Model);

            Assert.Equal(jewelries.Count, model.Items.Count);
            Assert.Equal(jewelries[0].Description, model.Items[0].Description);
        }
    }
}
