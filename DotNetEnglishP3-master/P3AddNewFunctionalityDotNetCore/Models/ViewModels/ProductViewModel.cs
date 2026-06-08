using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel : IValidatableObject
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingStock")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.Services.ProductService), ErrorMessageResourceName = "MissingPrice")]
        public string Price { get; set; }

        /// <summary>
        /// Validate if Price is a number and greater than zero, and if Stock is an integer and greater than zero.
        /// </summary>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>A collection of validation results</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Price = Price.Replace(",", ".").Trim();

            if (!decimal.TryParse(
                    Price,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal parsedPrice))
            {
                yield return new ValidationResult(
                    Resources.Models.Services.ProductService.PriceNotANumber,
                    new[] { nameof(Price) });
            }
            else if (parsedPrice <= 0)
            {
                yield return new ValidationResult(
                    Resources.Models.Services.ProductService.PriceNotGreaterThanZero,
                    new[] { nameof(Price) });
            }

            if (!int.TryParse(
                    Stock,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out int parsedStock))
            {
                yield return new ValidationResult(
                    Resources.Models.Services.ProductService.StockNotAnInteger,
                    new[] { nameof(Stock) });
            }
            else if (parsedStock <= 0)
            {
                yield return new ValidationResult(
                    Resources.Models.Services.ProductService.StockNotGreaterThanZero,
                    new[] { nameof(Stock) });
            }
        }
    }
}
