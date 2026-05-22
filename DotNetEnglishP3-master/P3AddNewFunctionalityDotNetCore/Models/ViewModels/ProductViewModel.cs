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

        [Required(ErrorMessage = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessage = "MissingQuantity")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "MissingPrice")]
        public string Price { get; set; }

        /// <summary>
        /// Validate if Price is a number and greater than zero, and if Stock is an integer and greater than zero.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!decimal.TryParse(
                    Price,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal parsedPrice))
            {
                yield return new ValidationResult(
                    "PriceNotANumber",
                    new[] { nameof(Price) });
            }
            else if (parsedPrice <= 0)
            {
                yield return new ValidationResult(
                    "PriceNotGreaterThanZero",
                    new[] { nameof(Price) });
            }

            if (!int.TryParse(
                    Stock,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out int parsedStock))
            {
                yield return new ValidationResult(
                    "StockNotAnInteger",
                    new[] { nameof(Stock) });
            }
            else if (parsedStock <= 0)
            {
                yield return new ValidationResult(
                    "StockNotGreaterThanZero",
                    new[] { nameof(Stock) });
            }
        }
    }
}
