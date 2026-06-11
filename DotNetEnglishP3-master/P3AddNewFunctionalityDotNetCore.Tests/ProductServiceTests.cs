using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
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
            var product = new ProductViewModel { Name = "" };
            var localizedMissingName = Resources.Models.Services.ProductService.MissingName;

            // Act
            var nameErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Name));

            // Assert
            Assert.NotEmpty(nameErrors);
            Assert.Equal(localizedMissingName, nameErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Name_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Name = "ProduitTest" };

            // Act
            var nameErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Name));

            // Assert
            Assert.Empty(nameErrors);
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
            var product = new ProductViewModel { Price = "" };
            var localizedMissingPrice = Resources.Models.Services.ProductService.MissingPrice;

            // Act
            var priceErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Price));

            // Assert
            Assert.NotEmpty(priceErrors);
            Assert.Equal(localizedMissingPrice, priceErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsNotValid_NotDecimal()
        {
            // Arrange
            var product = new ProductViewModel { Price = "Test" };
            var localizedMissingPriceNotANumber = Resources.Models.Services.ProductService.PriceNotANumber;

            // Act
            var priceErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Price));

            // Assert
            Assert.NotEmpty(priceErrors);
            Assert.Equal(localizedMissingPriceNotANumber, priceErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsNotValid_NotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel { Price = "-9.99" };
            var localizedMissingPriceNotGreaterThanZero = Resources.Models.Services.ProductService.PriceNotGreaterThanZero;

            // Act
            var priceErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Price));

            // Assert
            Assert.NotEmpty(priceErrors);
            Assert.Equal(localizedMissingPriceNotGreaterThanZero, priceErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Price = "9,99" };

            // Act
            var priceErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Price));

            // Assert
            Assert.Empty(priceErrors);
        }

        /// <summary>
        /// Check all cases for the validation of the Stock property of the ProductViewModel, which is required, must be an integer and greater than zero.
        /// </summary>
        [Fact]
        public void ProductViewModel_Stock_Valid_AllCases()
        {
            ProductViewModel_Stock_IsNotValid_Empty();
            ProductViewModel_Stock_IsNotValid_NotInteger_BecauseString();
            ProductViewModel_Stock_IsNotValid_NotInteger_BecauseDecimal();
            ProductViewModel_Stock_IsNotValid_NotGreaterThanZero();
            ProductViewModel_Stock_IsValid();
        }
        private void ProductViewModel_Stock_IsNotValid_Empty()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "" };
            var localizedMissingMissingStock = Resources.Models.Services.ProductService.MissingStock;

            // Act
            var stockErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.NotEmpty(stockErrors);
            Assert.Equal(localizedMissingMissingStock, stockErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Stock_IsNotValid_NotInteger_BecauseString()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "Test" };
            var localizedMissingStockNotAnInteger = Resources.Models.Services.ProductService.StockNotAnInteger;

            // Act
            var stockErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.NotEmpty(stockErrors);
            Assert.Equal(localizedMissingStockNotAnInteger, stockErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Stock_IsNotValid_NotInteger_BecauseDecimal()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "9.99" };
            var localizedMissingStockNotAnInteger = Resources.Models.Services.ProductService.StockNotAnInteger;

            // Act
            var stockErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.NotEmpty(stockErrors);
            Assert.Equal(localizedMissingStockNotAnInteger, stockErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Stock_IsNotValid_NotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "-10" };
            var localizedMissingStockNotGreaterThanZero = Resources.Models.Services.ProductService.StockNotGreaterThanZero;

            // Act
            var stockErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.NotEmpty(stockErrors);
            Assert.Equal(localizedMissingStockNotGreaterThanZero, stockErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Stock_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "10" };

            // Act
            var stockErrors = GetTargetPropertyErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.Empty(stockErrors);
        }

        /// <summary>
        /// Get all validation errors for a specific property of the ProductViewModel 
        /// by using the CheckProductValidationResult method of the ProductService, 
        /// which includes both data annotations and custom validation logic.
        /// </summary>
        /// <param name="product">The product view model to validate.</param>
        /// <param name="targetProperty">The name of the property to get validation errors for.</param>
        /// <returns>A list of validation results for the specified property.</returns>
        private List<ValidationResult> GetTargetPropertyErrors(ProductViewModel product, string targetProperty)
        {
            var productService = new ProductService(null, null, null, null);

            return productService.CheckProductValidationResult(product)
                .Where(r => r.MemberNames != null && r.MemberNames.Contains(targetProperty)).ToList();
        }


        /// <summary>
        /// Test the Create method of the ProductController to ensure that a new product can be created successfully in the database when valid data is provided, and that the product is properly cleaned up after the test to avoid polluting the database with test data.
        /// </summary>
        [Fact]
        public void ProductController_Create_Product()
        {
            // Arrange
            var context = GetDBContext();
            var productService = GetProductService(context);

            var languageServiceMock = Mock.Of<ILanguageService>();
            var productController = new ProductController(productService, languageServiceMock);

            var uniqueName = $"TestProduct_{Guid.NewGuid():N}";
            var product = new ProductViewModel
            {
                Name = uniqueName,
                Price = "12.34",
                Stock = "5",
                Description = "Description de test",
                Details = "Détails de test"
            };

            Product newProduct = null;

            try
            {
                // Act
                productController.Create(product);

                newProduct = context.Product.FirstOrDefault(p => p.Name == uniqueName);

                // Assert
                Assert.NotNull(newProduct);
            }
            finally // Cleanup
            {
                if (newProduct != null)
                {
                    context.Product.Remove(newProduct);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Test the DeleteProduct method of the ProductController to ensure that a product can be deleted successfully from the database.
        /// </summary>
        [Fact]
        public void ProductController_Delete_Product()
        {
            // Arrange
            var context = GetDBContext();
            var productService = GetProductService(context);

            var languageServiceMock = Mock.Of<ILanguageService>();
            var productController = new ProductController(productService, languageServiceMock);

            var uniqueName = $"TestProduct_{Guid.NewGuid():N}";
            var product = new Product
            {
                Name = uniqueName,
                Price = 10.5,
                Quantity = 3,
                Description = "Description de test",
                Details = "Détails de test"
            };

            Product productTarget = null;

            try
            {
                // Act
                context.Product.Add(product);
                context.SaveChanges();

                productTarget = context.Product.FirstOrDefault(p => p.Name == uniqueName);

                productController.DeleteProduct(productTarget.Id);
                productTarget = context.Product.FirstOrDefault(p => p.Name == uniqueName);

                // Assert
                Assert.Null(productTarget);
            }
            finally // Cleanup : if needed deletes the productTarget to avoid polluting the database if needed
            {
                if (productTarget != null)
                {
                    context.Product.Remove(productTarget);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Retrieve the database context using the connection string from appsettings.json, which allows the tests to interact with the actual database.
        /// Note: This test assumes that the database is properly set up and that the connection string is correct. It may require additional configuration or permissions to run successfully.
        /// </summary>
        /// <returns>The database context for the P3Referential database.</returns>
        private P3Referential GetDBContext()
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(appSettingsPath);

            // fallback to environment variables
            var configuration = configBuilder.Build();

            var connectionString = configuration.GetConnectionString("P3Referential");

            Assert.False(string.IsNullOrWhiteSpace(connectionString), "Connection string 'P3Referential' not found.");

            // Configure DbContextOptions to point to the real database
            var options = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(connectionString).Options;

            // Create context (constructor requires IConfiguration)
            var context = new P3Referential(options, configuration);

            return context;
        }

        /// <summary>
        /// Retrieve an instance of the ProductService using the provided database context,
        /// </summary>
        /// <param name="context">The database context to be used by the ProductService.</param>
        /// <returns>An instance of the ProductService.</returns>
        private ProductService GetProductService(P3Referential context)
        {
            var productRepository = new ProductRepository(context);
            var cart = new Cart();
            var orderRepositoryMock = Mock.Of<IOrderRepository>();
            var localizerMock = Mock.Of<IStringLocalizer<ProductService>>();

            return new ProductService(cart, productRepository, orderRepositoryMock, localizerMock);
        }
    }
}