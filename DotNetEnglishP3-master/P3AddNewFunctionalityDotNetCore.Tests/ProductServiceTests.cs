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
        /// Check all cases for the validation of the Name property of the ProductViewModel, which is required and cannot be empty.
        /// </summary>
        [Fact]
        public void ProductViewModel_Name_Valid_AllCases()
        {
            ProductViewModel_Name_IsNotValid_Empty();
            ProductViewModel_Name_IsValid();
        }
        private void ProductViewModel_Name_IsNotValid_Empty()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "",
                Price = "9.99",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.False(isValid, "ProductViewModel should be invalid beacause Name is empty");
            Assert.NotEmpty(results);
            Assert.Equal("MissingName", results[0].ErrorMessage);
        }
        private void ProductViewModel_Name_IsValid()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Id = 1,
                Name = "ProduitTest",
                Price = "9.99",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.True(isValid, "ProductViewModel Name should be valid");
            Assert.Empty(results);
        }

        /// <summary>
        /// Check all cases for the validation of the Price property of the ProductViewModel, which is required, must be a number and greater than zero.
        /// </summary>
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
                Price = "9.99",
                Stock = "10"
            };

            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();
            var isValid = false;

            // Act
            isValid = Validator.TryValidateObject(product, context, results, validateAllProperties: true);

            // Assert
            Assert.True(isValid, "ProductViewModel Price should be valid");
            Assert.Empty(results);
        }
    }
}