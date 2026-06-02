using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.Globalization;
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
            var nameErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Name));

            // Assert
            Assert.NotEmpty(nameErrors);
            Assert.Equal(localizedMissingName, nameErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Name_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Name = "ProduitTest" };

            // Act
            var nameErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Name));

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
            var priceErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Price));

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
            var priceErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Price));

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
            var priceErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Price));

            // Assert
            Assert.NotEmpty(priceErrors);
            Assert.Equal(localizedMissingPriceNotGreaterThanZero, priceErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Price_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Price = "9.99" };

            // Act
            var priceErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Price));

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
            var stockErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Stock));

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
            var stockErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Stock));

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
            var stockErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Stock));

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
            var stockErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.NotEmpty(stockErrors);
            Assert.Equal(localizedMissingStockNotGreaterThanZero, stockErrors[0].ErrorMessage);
        }
        private void ProductViewModel_Stock_IsValid()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "10" };

            // Act
            var stockErrors = GetAllPropertiesErrors(product, nameof(ProductViewModel.Stock));

            // Assert
            Assert.Empty(stockErrors);
        }


        /// <summary>
        /// Get all validation errors for a specific property of the ProductViewModel, including both data annotations and custom validation logic.
        /// </summary>
        /// <param name="product">The product view model to validate.</param>
        /// <param name="targetProperty">The name of the property to get validation errors for.</param>
        /// <returns>A list of validation results for the specified property.</returns>
        private List<ValidationResult> GetAllPropertiesErrors(ProductViewModel product, string targetProperty)
        {
            var context = new ValidationContext(product);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(product, context, results);

            var customResults = product.Validate(context).ToList();

            foreach (var customResult in customResults)
            {
                results.Add(customResult);
            }

            var targetErrors = results.Where(r => r.MemberNames != null && r.MemberNames.Contains(targetProperty)).ToList();

            return targetErrors;
        }

        [Fact]
        public void CreateProductTest()
        {
            // Trouve un appsettings.json dans l'arborescence (jusqu'à 6 niveaux)
            string FindAppSettings()
            {
                var dir = Directory.GetCurrentDirectory();
                for (int i = 0; i < 6 && dir != null; i++)
                {
                    var candidate = Path.Combine(dir, "appsettings.json");
                    if (File.Exists(candidate))
                        return candidate;
                    var parent = Directory.GetParent(dir);
                    dir = parent?.FullName;
                }
                return null;
            }

            var appSettingsPath = FindAppSettings();

            var configBuilder = new ConfigurationBuilder();
            if (appSettingsPath != null)
            {
                configBuilder.AddJsonFile(appSettingsPath, optional: false, reloadOnChange: false);
            }
            // fallback to environment variables
            configBuilder.AddEnvironmentVariables();
            var configuration = configBuilder.Build();

            var connectionString = configuration.GetConnectionString("P3Referential")
                                   ?? Environment.GetEnvironmentVariable("ConnectionStrings__P3Referential");

            Assert.False(string.IsNullOrWhiteSpace(connectionString), "Connection string 'P3Referential' introuvable. Ajoute-la dans appsettings.json ou en variable d'environnement.");

            // Configure le DbContextOptions en pointant sur la base réelle
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            // Crée le contexte (le constructeur exige IConfiguration)
            var context = new P3Referential(options, configuration);

            // Assure que la base est accessible (ne pas créer de schéma si non désiré)
            // context.Database.EnsureCreated(); // <-- active si nécessaire

            // Instancie repository / service réels
            var productRepository = new ProductRepository(context);
            var cart = new Cart();
            var orderRepositoryMock = Mock.Of<IOrderRepository>();
            var localizerMock = Mock.Of<IStringLocalizer<ProductService>>();

            var productService = new ProductService(cart, productRepository, orderRepositoryMock, localizerMock);

            var languageServiceMock = Mock.Of<ILanguageService>();
            var productController = new ProductController(productService, languageServiceMock);

            // Crée un produit de test unique (nom avec GUID pour éviter collisions)
            var uniqueName = $"TestProduct_{Guid.NewGuid():N}";
            var productVm = new ProductViewModel
            {
                Name = uniqueName,
                Price = "12,34",
                Stock = "5",
                Description = "Description de test",
                Details = "Détails de test"
            };

            // Act: appeler l'action Create du controller (POST)
            var result = productController.Create(productVm);

            // Assert: vérifie que le produit a bien été ajouté en base
            var added = context.Product.FirstOrDefault(p => p.Name == uniqueName);
            Assert.NotNull(added);

            try
            {
                // Cleanup : supprimer le produit ajouté pour ne pas polluer la base
                if (added != null)
                {
                    context.Product.Remove(added);
                    context.SaveChanges();
                }
            }
            finally
            {
                // dispose context si nécessaire
                (context as IDisposable)?.Dispose();
            }
        }

        [Fact]
        public void ProductController_Create_Delete_Product()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUiCulture = CultureInfo.CurrentUICulture;

            try
            {
                var testCulture = new CultureInfo("en-EN");
                CultureInfo.CurrentCulture = testCulture;
                CultureInfo.CurrentUICulture = testCulture;

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


                // Instantiate repository / real service
                var productRepository = new ProductRepository(context);
                var cart = new Cart();
                var orderRepositoryMock = Mock.Of<IOrderRepository>();
                var localizerMock = Mock.Of<IStringLocalizer<ProductService>>();

                var productService = new ProductService(cart, productRepository, orderRepositoryMock, localizerMock);

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

                // Act
                productController.Create(product);

                var added = context.Product.FirstOrDefault(p => p.Name == uniqueName);
                
                productController.DeleteProduct(added.Id);
                added = context.Product.FirstOrDefault(p => p.Name == uniqueName);

                try
                {
                    // Assert
                    Assert.Null(added);
                }
                finally // Cleanup : if needed deletes the product added to avoid polluting the database if needed
                {
                    if (added != null)
                    {
                        context.Product.Remove(added);
                        context.SaveChanges();
                    }
                }
            }
            finally 
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUiCulture;
            }
        }
    }
}