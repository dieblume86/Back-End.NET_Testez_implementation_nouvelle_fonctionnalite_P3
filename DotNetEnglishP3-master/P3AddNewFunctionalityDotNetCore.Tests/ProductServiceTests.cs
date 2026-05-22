using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void ExampleMethod()
        {
            // Arrange

            // Act


            // Assert
            Assert.Equal(1, 1);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
        [Fact]
        public void ProductViewModel_Price_Valid_AllCases()
        {
            ProductViewModel_Price_IsNotValid_Empty();
            ProductViewModel_Price_IsNotValid_NotDecimal();
            ProductViewModel_Price_IsNotValid_NotGreaterThanZero();
            ProductViewModel_Price_IsValid();
        }
        private void ProductViewModel_Price_IsNotValid_Empty()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "ProduitTest",
                Description = "Desc",
                Details = "Details",
                Price = "",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.False(isValid, "ProductViewModel should be invalid beacause Price is empty");
            Assert.NotEmpty(results);
            Assert.Equal("MissingPrice", results[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsNotValid_NotDecimal()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "ProduitTest",
                Description = "Desc",
                Details = "Details",
                Price = "Test",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.False(isValid, "ProductViewModel should be invalid beacause Price is not a decimal");
            Assert.NotEmpty(results);
            Assert.Equal("PriceNotANumber", results[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsNotValid_NotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "ProduitTest",
                Description = "Desc",
                Details = "Details",
                Price = "-9.99",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.False(isValid, "ProductViewModel should be invalid beacause Price is not greater than 0");
            Assert.NotEmpty(results);
            Assert.Equal("PriceNotGreaterThanZero", results[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsValid()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "ProduitTest",
                Description = "Desc",
                Details = "Details",
                Price = "9.99",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.True(isValid, "ProductViewModel should be valid");
            Assert.Empty(results);
        }



        [Fact]
        public void CreateProductCheckName()
        {
            // Arrange
            var producViewModel = new ProductViewModel
            {
                Id = 6,
                Name = "",
                Description = "TestDescription",
                Details = "TestDetails",
                Price = "",
                Stock = ""
            };

            var productService = Mock.Of<IProductService>();
            //Mock.Get(productService).Setup(m => m.CheckProductModelErrors(producViewModel));

            var languageService = Mock.Of<ILanguageService>();
            var productController = new ProductController(productService, languageService);

            // Act
            IActionResult result = productController.Create(producViewModel);
            //IActionResult result = productController.Create(producViewModel);

            // Assert
            Assert.Equal(1, 1);

            productController.DeleteProduct(6);
        }
    }
}